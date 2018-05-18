using Microsoft.AspNetCore.Mvc;
using Salary.DataAccess;
using Salary.Models;

namespace Salary.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return _employeeRepository.Get(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Employee employee)
        {
            return Ok(new
            {
                employee.Name,
                Id = _employeeRepository.Create(employee)
            });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Route("UpdateAddress")]
        public IActionResult Put(int id, [FromBody]string address)
        {
            return Ok(_employeeRepository.UpdateAddress(id, address));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_employeeRepository.Delete(id));
        }
    }
}
