namespace clean_arch.Domain.DTO.Auth;

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty!;
    public string Email { get; set; } = string.Empty!;
    public string PhoneNumber { get; set; } = string.Empty!;
    public string Password { get; set; } = string.Empty!;
}