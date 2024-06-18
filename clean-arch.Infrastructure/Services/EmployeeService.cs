using clean_arch.Domain.DTO;
using clean_arch.Domain.Entities;
using clean_arch.Domain.Helpers;
using clean_arch.Infrastructure.Interfaces;
using clean_arch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace clean_arch.Infrastructure.Services;

internal class EmployeeService : IEmployee
{
    internal readonly ILogger<EmployeeService> _logger;
    internal ApplicationDbContext _ctx;

    public EmployeeService(ApplicationDbContext context, ILogger<EmployeeService> iLogger)
    {
        _ctx = context;
        _logger = iLogger;
    }

    public async Task<List<EmployeeDTO>> GetAllEmployees()
    {
        var employees = await _ctx.Employees.ToListAsync();
        return employees.Select(EmployeeHelpers.MapEmployeeToEmployeeDTO).ToList();
    }

    public async Task<int> CreateEmployee(EmployeeRequest request)
    {
        var employee = EmployeeHelpers.MapEmployeeRequestToEmployee(request);

        await _ctx.Employees.AddAsync(employee);
        await _ctx.SaveChangesAsync();
        return employee.Id;
    }

    public async Task<bool> DeleteEmployeeById(int id)
    {
        var employee = await _ctx.Employees.FirstAsync(employee => employee.Id == id);

        _ctx.Entry<Employee>(employee).State = EntityState.Deleted;

        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEmployeeByEmail(string email)
    {
        var employee = await _ctx.Employees.FirstAsync(employee => employee.Email.Equals(email));

        _ctx.Entry<Employee>(employee).State = EntityState.Deleted;

        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<EmployeeDTO> GetEmployeeById(int id)
    {
        var employee = await _ctx.Employees.FirstAsync(employee => employee.Id == id);
        return EmployeeHelpers.MapEmployeeToEmployeeDTO(employee);
    }

    public async Task<EmployeeDTO> GetEmployeeByEmail(string email)
    {
        var employee = await _ctx.Employees.FirstAsync(employee => employee.Email.Equals(email));
        return EmployeeHelpers.MapEmployeeToEmployeeDTO(employee);
    }

    public async Task<EmployeeDTO> UpdateEmployee(int id, EmployeeRequest request)
    {
        var employee = await _ctx.Employees.FirstAsync(employee => employee.Id == id);
        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.Phone = request.Phone;
        employee.AlternativePhone = request.AlternativePhone ?? "";
        employee.Gender = request.Gender;
        employee.DateOfBirth = request.DateOfBirth;
        employee.UpdatedAt = DateTime.Now;

        _ctx.Entry<Employee>(employee).State = EntityState.Modified;
        await _ctx.SaveChangesAsync();

        return EmployeeHelpers.MapEmployeeToEmployeeDTO(employee);
    }
}