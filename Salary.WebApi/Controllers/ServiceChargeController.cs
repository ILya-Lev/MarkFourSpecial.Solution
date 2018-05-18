using Microsoft.AspNetCore.Mvc;
using Salary.DataAccess;
using Salary.Models;
using System;

namespace Salary.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/ServiceCharge")]
    public class ServiceChargeController : Controller
    {
        private readonly IEntityForEmployeeRepository<ServiceCharge> _serviceChargeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ServiceChargeController(IEntityForEmployeeRepository<ServiceCharge> serviceChargeRepository, IEmployeeRepository employeeRepository)
        {
            _serviceChargeRepository = serviceChargeRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{employeeId}")]
        public IActionResult Get(int employeeId, DateTime? since, DateTime? until)
        {
            return Ok(_serviceChargeRepository.GetForEmployee(employeeId, since, until));
        }

        [HttpGet("serviceCharge/{id}", Name = "getServiceCharge")]
        public IActionResult Get(int id)
        {
            return Ok(_serviceChargeRepository.Get(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ServiceCharge serviceCharge)
        {
            var employee = _employeeRepository.Get(serviceCharge.EmployeeId);
            if (employee.TradeUnionCharge == null)
            {
                return BadRequest(new
                {
                    serviceCharge.EmployeeId,
                    Message = $"{employee.Name} is not a member of a trade union and cannot be charged." +
                              $" Either make this employee a trade union member or specify another employee."
                });
            }

            if (serviceCharge.Amount <= 0)
                return BadRequest($"Service charge amount cannot be non-positive.");

            var id = _serviceChargeRepository.Create(serviceCharge);
            return Created(Url.Link("getServiceCharge", new { id }), new { id, serviceCharge.Amount });
        }
    }
}