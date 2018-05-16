using System;
using Salary.DataAccess;

namespace Salary.Services.Implementation.PayrollStrategies
{
    public class SalaryPayrollStrategy : IPayrollStrategy
    {
        protected readonly IEmployeeRepository _employeeRepository;

        public SalaryPayrollStrategy(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public virtual decimal GetPayroll(int employeeId, DateTime forDate)
        {
            if (!IsLastDayOfMonth(forDate))
            {
                return 0m;
            }

            var employee = _employeeRepository.Get(employeeId);
            return employee.MajorRate;
        }

        private static bool IsLastDayOfMonth(DateTime forDate)
        {
            var nextDay = forDate.AddDays(1);
            return nextDay.Month == forDate.Month + 1
                || nextDay.Year == forDate.Year + 1;
        }
    }
}
