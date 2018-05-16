using Salary.DataAccess;
using Salary.Models;
using System;
using System.Linq;

namespace Salary.Services.Implementation
{
    public class HourlyPayrollStrategy : IPayrollStrategy
    {
        private const int StandardHours = 8;
        private const decimal OvertimeFactor = 1.5m;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEntityForEmployeeRepository<TimeCard> _timeCardRepository;
        private readonly IEntityForEmployeeRepository<SalaryPayment> _salaryPaymentRepository;

        public HourlyPayrollStrategy(IEmployeeRepository employeeRepository, IEntityForEmployeeRepository<TimeCard> timeCardRepository, IEntityForEmployeeRepository<SalaryPayment> salaryPaymentRepository)
        {
            _employeeRepository = employeeRepository;
            _timeCardRepository = timeCardRepository;
            _salaryPaymentRepository = salaryPaymentRepository;
        }

        public decimal GetPayroll(int employeeId, DateTime forDate)
        {
            var employee = _employeeRepository.Get(employeeId);

            var payments = _salaryPaymentRepository.GetForEmployee(employeeId, null, DateTime.Now);
            var lastTimePayed = payments.OrderByDescending(sp => sp.Date).FirstOrDefault()?.Date;
            var timeCards = _timeCardRepository.GetForEmployee(employeeId, lastTimePayed, DateTime.Now);

            return timeCards.Select(tc => EffectiveHours(tc.Hours) * employee.MajorRate).Sum();
        }

        private static decimal EffectiveHours(float hours)
        {
            return hours > StandardHours
                ? (decimal)(hours - StandardHours) * OvertimeFactor + StandardHours
                : (decimal)hours;
        }
    }
}
