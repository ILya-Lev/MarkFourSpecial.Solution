using Salary.Models;
using System;

namespace Salary.Services
{
    public interface IPayrollStrategy
    {
        PaymentType PaymentType { get; }
        decimal GetPayroll(int employeeId, DateTime forDate);
    }

    public interface IHourlyPayrollStrategy : IPayrollStrategy { }
    public interface IMonthlyPayrollStrategy : IPayrollStrategy { }
    public interface ICommissionedPayrollStrategy : IPayrollStrategy { }
}