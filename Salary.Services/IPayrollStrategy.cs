using System;

namespace Salary.Services
{
    public interface IPayrollStrategy
    {
        decimal GetPayroll(int employeeId, DateTime forDate);
    }
}