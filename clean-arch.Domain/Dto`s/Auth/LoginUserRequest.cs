namespace clean_arch.Domain.DTO.Auth;

public class LoginUserRequest
{
    public string Password { get; set; } = string.Empty!;
    public string Email { get; set; } = string.Empty!;

    public string? TwoFactorCode { get; set; } = string.Empty;

    public string? TwoFactorRecoveryCode { get; set; } = string.Empty;
}