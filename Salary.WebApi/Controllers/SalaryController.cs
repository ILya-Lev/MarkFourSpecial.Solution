using Microsoft.AspNetCore.Mvc;
using Salary.DataAccess;
using Salary.Services;
using System;
using System.Linq;

namespace Salary.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Salary")]
    public class SalaryController : Controller
    {
        private readonly ISalaryCalculationService _calculationService;
        private readonly IEmployeeRepository _employeeRepository;

        public SalaryController(ISalaryCalculationService calculationService, IEmployeeRepository employeeRepository)
        {
            _calculationService = calculationService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetAllSalaries(DateTime? date)
        {
            var salaries = _employeeRepository.GetAll()
                .AsParallel()
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    Payment = _calculationService.GetSalary(e.Id, date ?? DateTime.Now)
                })
                .ToArray();

            return Ok(salaries);
        }

        [HttpGet("{id}")]
        public IActionResult GetSalaryForEmployee(int id, DateTime? date)
        {
            var salaryData = new
            {
                Id = id,
                _employeeRepository.Get(id).Name,
                Payment = _calculationService.GetSalary(id, date ?? DateTime.Now)
            };

            return Ok(salaryData);
        }
    }
}