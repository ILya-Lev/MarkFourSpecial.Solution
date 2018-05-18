using Microsoft.AspNetCore.Mvc;
using Salary.DataAccess;
using Salary.Models;
using System;
using System.Linq;

namespace Salary.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/TimeCard")]
    public class TimeCardController : Controller
    {
        private readonly IEntityForEmployeeRepository<TimeCard> _timeCardRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public TimeCardController(IEntityForEmployeeRepository<TimeCard> timeCardRepository, IEmployeeRepository employeeRepository)
        {
            _timeCardRepository = timeCardRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{employeeId}")]
        public IActionResult Get(int employeeId, DateTime? since, DateTime? until)
        {
            return Ok(_timeCardRepository.GetForEmployee(employeeId, since, until));
        }

        [HttpGet("card/{employeeId}", Name = "getTimeCard")]
        public IActionResult GetCard(int employeeId, DateTime? since, DateTime? until)
        {
            return Ok(_timeCardRepository.GetForEmployee(employeeId, since, until));
        }

        [HttpPost]
        public IActionResult Post([FromBody] TimeCard timeCard)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                                    ? e.Exception.InnerException?.Message
                                    : e.ErrorMessage));

            _employeeRepository.Get(timeCard.EmployeeId);

            var id = _timeCardRepository.Create(timeCard);
            return Created(Url.Link("getTimeCard", new { employeeId = id }), new { id, timeCard.Hours });
        }
    }
}