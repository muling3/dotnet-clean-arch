using clean_arch.Domain.DTO;
using clean_arch.Domain.Entities;

namespace clean_arch.Domain.Helpers;

public class EmployeeHelpers
{

    public static EmployeeDTO MapEmployeeToEmployeeDTO(Employee employee)
    {
        return new EmployeeDTO
        {
            DisplayName = employee.DisplayName,
            Email = employee.Email,
            Gender = employee.Gender,
            Phone = employee.Phone,
        };
    }

    public static Employee MapEmployeeRequestToEmployee(EmployeeRequest request)
    {
        return new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DisplayName = request.FirstName.Trim() + " " + request.LastName.Trim(),
            Email = request.Email,
            Phone = request.Phone,
            AlternativePhone = request.AlternativePhone ?? "",
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth.ToUniversalTime(),
        };
    }
}