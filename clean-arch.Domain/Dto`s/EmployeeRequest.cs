using clean_arch.Domain.Enums;

namespace clean_arch.Domain.DTO;

public class EmployeeRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? AlternativePhone { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
}