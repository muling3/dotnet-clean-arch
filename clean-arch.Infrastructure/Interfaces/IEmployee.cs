using clean_arch.Domain.DTO;

namespace clean_arch.Infrastructure.Interfaces;

public interface IEmployee
{
    Task<int> CreateEmployee(EmployeeRequest request);
    Task<List<EmployeeDTO>> GetAllEmployees();
    Task<EmployeeDTO> GetEmployeeById(int id);
    Task<EmployeeDTO> GetEmployeeByEmail(string email);
    Task<EmployeeDTO> UpdateEmployee(int id, EmployeeRequest request);
    Task<bool> DeleteEmployeeById(int id);
    Task<bool> DeleteEmployeeByEmail(string email);
}