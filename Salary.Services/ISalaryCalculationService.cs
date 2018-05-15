using System;

namespace Salary.Services
{
    public interface ISalaryCalculationService
    {
        decimal GetSalary(int employeeId, DateTime forDate);
    }
}
