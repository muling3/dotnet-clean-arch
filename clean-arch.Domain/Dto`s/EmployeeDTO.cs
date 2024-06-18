using clean_arch.Domain.Enums;

namespace clean_arch.Domain.DTO;

public class EmployeeDTO
{
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Gender Gender { get; set; }
}