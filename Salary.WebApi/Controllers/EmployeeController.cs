using Microsoft.AspNetCore.Mvc;
using Salary.DataAccess;
using Salary.Models;
using System.Diagnostics;

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

        [HttpGet("{id}", Name = "get")]
        public Employee Get(int id)
        {
            return _employeeRepository.Get(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Employee employee)
        {
            var creationResult = new
            {
                employee.Name,
                Id = _employeeRepository.Create(employee)
            };

            return new CreatedResult(Url.Link("get", new { id = creationResult.Id }), creationResult);
        }

        [HttpPut("UpdateName/{id}")]
        public IActionResult PutName(int id, [FromBody]string name)
        {
            return Ok(_employeeRepository.UpdateName(id, name));
        }

        [HttpPut("UpdateAddress/{id}")]
        public IActionResult PutAddress(int id, [FromBody]string address)
        {
            return Ok(_employeeRepository.UpdateAddress(id, address));
        }

        [HttpPut("UpdateHourly/{id}")]
        public IActionResult PutHourly(int id, [FromBody]decimal rate)
        {
            return Ok(_employeeRepository.UpdateHourly(id, rate));
        }

        [HttpPut("UpdateSalaried/{id}")]
        public IActionResult PutSalaried(int id, [FromBody]decimal amount)
        {
            return Ok(_employeeRepository.UpdateSalaried(id, amount));
        }

        [HttpPut("UpdateCommissioned/{id}")]
        public IActionResult PutCommissioned(int id, [FromBody]CommissionedRates rates)
        {
            return Ok(_employeeRepository.UpdateCommissioned(id, rates.Major, rates.Minor));
        }

        [HttpPut("UpdateTradeUnionCharge/{id}")]
        public IActionResult PutTradeUnionCharge(int id, [FromBody]decimal? charge)
        {
            return Ok(_employeeRepository.UpdateTradeUnionCharge(id, charge));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_employeeRepository.Delete(id));
        }
    }

    [DebuggerDisplay("{" + nameof(Major) + ("}; {" + nameof(Minor) + "}"))]
    public class CommissionedRates
    {
        public decimal Major { get; set; }
        public decimal Minor { get; set; }
    }
}
