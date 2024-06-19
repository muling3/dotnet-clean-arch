using Microsoft.AspNetCore.Mvc;
using clean_arch.Infrastructure.Interfaces;
using clean_arch.Domain.DTO;
using Microsoft.AspNetCore.Authorization;

namespace clean_arch.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployee _employeeService;

    public EmployeeController(IEmployee emplSvc)
    {
        _employeeService = emplSvc;
    }

    [HttpGet(Name = "Get employee")]
    public async Task<IActionResult> GetEmployeeByIdOrEmailAsync([FromQuery] int? id, [FromQuery] string? email)
    {
        if (id.HasValue)
        {
            return Ok(await _employeeService.GetEmployeeById((int)id));
        }
        else if ((email ?? "").Trim().Length > 0)
        {
            return Ok(await _employeeService.GetEmployeeByEmail(email!));

        }

        return Ok(await _employeeService.GetAllEmployees());
    }

    [HttpPost(Name = "Create an Employee")]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] EmployeeRequest request)
    {
        var id = await _employeeService.CreateEmployee(request);
        return Created(nameof(GetEmployeeByIdOrEmailAsync), new { id, email = (string?)null });
    }

    [HttpPut(Name = "Update an Employee")]
    public async Task<IActionResult> UpdateEmployeeAsync([FromQuery] int id, [FromBody] EmployeeRequest request)
    {
        var employee = await _employeeService.UpdateEmployee(id, request);
        return Ok(employee);
    }

    [HttpDelete(Name = "Delete employee")]
    public async Task<IActionResult> DeleteEmployeeByIdOrEmailAsync([FromQuery] int? id, [FromQuery] string? email)
    {
        if (id.HasValue)
        {
            return Ok(await _employeeService.DeleteEmployeeById((int)id));
        }
        else if ((email ?? "").Trim().Length > 0)
        {
            return Ok(await _employeeService.DeleteEmployeeByEmail(email!));

        }

        return BadRequest("Employee Does Not exist");
    }
}