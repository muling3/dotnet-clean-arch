// using clean_arch.Infrastructure.Interfaces;
// using clean_arch.Infrastructure.Persistence;
// using clean_arch.Infrastructure.Services;
// using Microsoft.EntityFrameworkCore;
using clean_arch.Authentication.Persistence;
using clean_arch.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace clean_arch.Authentication.Extensions;

public static class ServiceExtensions
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("MainDb"), db => db.MigrationsAssembly("clean-arch.Infrastructure")));
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AuthDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            // table names 
            // options.Stores.
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });
        // services.AddScoped<IEmployee, EmployeeService>();

        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();
    }
}