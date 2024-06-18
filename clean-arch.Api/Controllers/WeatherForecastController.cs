using Microsoft.AspNetCore.Mvc;

namespace clean_arch.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{

    public EmployeeController() { }

    [HttpGet(Name = "hello")]
    public string HelloWorld()
    {
        return "HELLO  WORLD FROM EMPLOYEE CONTROLLER";
    }
}
