using System;

namespace Salary.Services
{
    public class SalaryCalculationService : ISalaryCalculationService
    {
        private readonly IPayrollStrategy _payrollStrategy;
        private readonly IChargeStrategy _chargeStrategy;

        public SalaryCalculationService(IPayrollStrategy payrollStrategy,
            IChargeStrategy chargeStrategy)
        {
            _payrollStrategy = payrollStrategy;
            _chargeStrategy = chargeStrategy;
        }

        public decimal GetSalary(int employeeId, DateTime forDate)
        {
            var payroll = _payrollStrategy.GetPayroll(employeeId, forDate);
            var charge = _chargeStrategy.GetCharge(employeeId, forDate);

            return payroll - charge;
        }
    }
}