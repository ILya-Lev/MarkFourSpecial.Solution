using Microsoft.AspNetCore.Mvc;
using Salary.DataAccess;
using Salary.Models;
using System;

namespace Salary.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/SalesReceipt")]
    public class SalesReceiptController : Controller
    {
        private readonly IEntityForEmployeeRepository<SalesReceipt> _salesReceiptRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public SalesReceiptController(IEntityForEmployeeRepository<SalesReceipt> salesReceiptRepository, IEmployeeRepository employeeRepository)
        {
            _salesReceiptRepository = salesReceiptRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{employeeId}")]
        public IActionResult Get(int employeeId, DateTime? since, DateTime? until)
        {
            return Ok(_salesReceiptRepository.GetForEmployee(employeeId, since, until));
        }

        [HttpGet("{id}", Name = "getSalesReceipt")]
        public IActionResult GetSalesReceipt(int id)
        {
            return Ok(_salesReceiptRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] SalesReceipt salesReceipt)
        {
            if (salesReceipt.Amount <= 0)
                return BadRequest($"Sales amount cannot be non-positive. Actual value '{salesReceipt.Amount}'.");

            var employee = _employeeRepository.Get(salesReceipt.EmployeeId);
            if (employee.PaymentType != PaymentType.Commissioned)
                return BadRequest(new
                {
                    salesReceipt.EmployeeId,
                    Payment = employee.PaymentType.ToString(),
                    Message = $"{employee.Name} does not get percent from sales." +
                              $" Either change his payment type, or change employeeId."
                });

            var id = _salesReceiptRepository.Create(salesReceipt);
            return Created(Url.Link("getSalesReceipt", new { id }), new { id, salesReceipt.Amount });
        }
    }
}