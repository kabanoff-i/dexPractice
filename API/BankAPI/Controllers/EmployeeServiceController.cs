using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ModelsDb;
using ServicesDb;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeServiceController : ControllerBase
    {
        EmployeeService employeeService;
        public EmployeeServiceController() 
        {
            employeeService = new EmployeeService();
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(Guid id)
        {
            Employee Employee = employeeService.GetEmployeeAsync(id).Result;
            return Ok(Employee);
        }
        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee cl)
        {
            employeeService.AddEmployeeAsync(cl);
            return Created(new Uri(Request.GetEncodedUrl() + "/" + cl.id), cl);
        }
        [HttpPut("{id}")]
        public IActionResult ChangeEmployee(Guid id)
        {
            employeeService.ChangeEmployeeAsync(id);
            return Ok();

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }

    }
}
