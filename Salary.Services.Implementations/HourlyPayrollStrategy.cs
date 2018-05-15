using System;

namespace Salary.Services.Implementation
{
    public class HourlyPayrollStrategy : IPayrollStrategy
    {
        public HourlyPayrollStrategy()
        {

        }
        public decimal GetPayroll(int employeeId, DateTime forDate)
        {
            throw new NotImplementedException();
        }
    }
}
