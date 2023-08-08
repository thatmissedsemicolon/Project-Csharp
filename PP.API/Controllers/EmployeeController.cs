using Microsoft.AspNetCore.Mvc;
using PP.API.EC;
using PP.Library.DTO;
using PP.Library.Utilities;

namespace PP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<EmployeeDTO> Get(string? query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new EmployeeEC().GetAllEmployees();
            }
            else
            {
                return new EmployeeEC().Search(query);
            }
        }

        [HttpPost("/Employee/add")]
        public EmployeeDTO AddEmployeeToClient([FromBody] EmployeeDTO employee)
        {
            return new EmployeeEC().Add(employee);
        }

        [HttpPost("/Employee/update")]
        public EmployeeDTO UpdateEmployeeInClient([FromBody] EmployeeDTO employee)
        {
            return new EmployeeEC().Update(employee);
        }

        [HttpGet("/Employee/{id}")]
        public EmployeeDTO GetEmployeeById(int id)
        {
            return new EmployeeEC().GetEmployeeById(id);
        }

        [HttpDelete("/Employee/delete/{employeeId}")]
        public bool DeleteEmployee(string employeeId)
        {
            return new EmployeeEC().DeleteById(employeeId);
        }

        [HttpPost("/Employee/search")]
        public IEnumerable<EmployeeDTO> SearchEmployees([FromBody] QueryMessage query)
        {
            return new EmployeeEC().Search(query.Query);
        }
    }
}
