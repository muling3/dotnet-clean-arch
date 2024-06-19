using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using clean_arch.Domain.DTO.Auth;
using clean_arch.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;

namespace clean_arch.Authentication.Controllers;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class AuthController : ControllerBase
{

    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    private readonly UserManager<User> userManager;

    private readonly RoleManager<Role> roleManager;

    private readonly ILogger<AuthController> _logger;

    private readonly IServiceProvider provider;

    private readonly LinkGenerator linkGenerator;

    private readonly IEmailSender<User> emailSender;

    private readonly SignInManager<User> signInManager;

    private readonly string confirmEmailEndpointName = "confirmEmail";

    public AuthController(ILogger<AuthController> logger, IServiceProvider serviceProvider, IEmailSender<User> sender)
    {
        _logger = logger;
        provider = serviceProvider;
        emailSender = sender;


        userManager = provider.GetRequiredService<UserManager<User>>();
        roleManager = provider.GetRequiredService<RoleManager<Role>>();
        linkGenerator = provider.GetRequiredService<LinkGenerator>();
        signInManager = provider.GetRequiredService<SignInManager<User>>();
    }

    [HttpGet]
    [Route("helloWorld", Name = "HelloWorld")]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [HttpGet]
    [Route("roles", Name = "GetAllRoles")]
    [AllowAnonymous]
    public IEnumerable<Role> GetRoles()
    {
        var roles = roleManager.Roles;

        return [.. roles];
    }

    [HttpPost]
    [Route("createRole", Name = "CreateRole")]
    [AllowAnonymous]
    public async Task<IEnumerable<Role>> CreateRole(string roleName)
    {
        //creating the roles and seeding them to the database
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var roleResult = await roleManager.CreateAsync(new Role { Name = roleName });
        }

        var roles = roleManager.Roles;

        return [.. roles];
    }

    [HttpGet]
    [Route("users", Name = "GetAllUsers")]
    [AllowAnonymous]
    public IEnumerable<User> GetUsers()
    {
        var users = userManager.Users;

        return [.. users];
    }

    [HttpPost]
    [Route("register", Name = "CreateUser")]
    [AllowAnonymous]
    public async Task<IResult> CreateUser(CreateUserRequest request)
    {
        // getting the current http context
        var context = HttpContext;

        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("requires a user store with email support.");
        }

        var userStore = provider.GetRequiredService<IUserStore<User>>();
        var emailStore = (IUserEmailStore<User>)userStore;
        var phoneStore = (IUserPhoneNumberStore<User>)userStore;
        var email = request.Email;

        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        var user = new User();
        await userStore.SetUserNameAsync(user, request.Username, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        await phoneStore.SetPhoneNumberAsync(user, request.PhoneNumber, CancellationToken.None);

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        // TODO: await SendConfirmationEmailAsync(user, userManager, context, email);

        return TypedResults.Created();
    }

    [HttpPost]
    [Route("login", Name = "LoginUser")]
    [AllowAnonymous]
    public async Task<IResult> LoginUser(LoginUserRequest request)
    {
        // set auth scheme
        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(request.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(request.TwoFactorCode, false, rememberClient: false);
            }
            else if (!string.IsNullOrEmpty(request.TwoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(request.TwoFactorRecoveryCode);
            }
        }
        _logger.LogInformation(" GOT HERE {}", result.ToString());

        if (!result.Succeeded)
        {
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        }

        return TypedResults.Empty;
    }


    [HttpPost]
    [Route("addRoleToUser", Name = "AddRoleToUser")]
    [AllowAnonymous]
    public async Task<IEnumerable<User>> AddRoleToUser(string roleName, string userEmail)
    {
        var role = await roleManager.RoleExistsAsync(roleName);
        if (!role)
        {
            throw new ArgumentException("Role does not exist");
        }

        var user = await userManager.FindByEmailAsync(userEmail) ?? throw new ArgumentException("User does not exist");

        await userManager.AddToRoleAsync(user, roleName);

        var users = userManager.Users;

        return [.. users];
    }

    private async Task SendConfirmationEmailAsync(User user, UserManager<User> userManager, HttpContext context, string email, bool isChange = false)
    {
        if (confirmEmailEndpointName is null)
        {
            throw new NotSupportedException("No email confirmation endpoint was registered!");
        }

        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync(user, email)
            : await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary()
        {
            ["userId"] = userId,
            ["code"] = code,
        };

        if (isChange)
        {
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add("changedEmail", email);
        }

        var confirmEmailUrl = linkGenerator.GetUriByName(context, confirmEmailEndpointName, routeValues)
            ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

        await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}
