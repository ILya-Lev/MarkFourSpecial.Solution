using System;
using System.Linq;
using Salary.DataAccess;
using Salary.Models;

namespace Salary.Services.Implementation.PayrollStrategies
{
    public class HourlyPayrollStrategy : IPayrollStrategy
    {
        private const int StandardHours = 8;
        private const decimal OvertimeFactor = 1.5m;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEntityForEmployeeRepository<TimeCard> _timeCardRepository;

        public HourlyPayrollStrategy(IEmployeeRepository employeeRepository,
            IEntityForEmployeeRepository<TimeCard> timeCardRepository)
        {
            _employeeRepository = employeeRepository;
            _timeCardRepository = timeCardRepository;
        }

        public decimal GetPayroll(int employeeId, DateTime forDate)
        {
            if (forDate.DayOfWeek != DayOfWeek.Friday)
            {
                return 0m;
            }

            var employee = _employeeRepository.Get(employeeId);

            var timeCards = _timeCardRepository.GetForEmployee(employeeId, forDate.Subtract(TimeSpan.FromDays(7)), forDate);

            return timeCards.Select(tc => EffectiveHours(tc.Hours)).Sum() * employee.MajorRate;
        }

        private static decimal EffectiveHours(float hours)
        {
            return hours > StandardHours
                ? (decimal)(hours - StandardHours) * OvertimeFactor + StandardHours
                : (decimal)hours;
        }
    }
}
