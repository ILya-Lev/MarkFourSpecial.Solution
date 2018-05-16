using System;

namespace Salary.Services
{
    public class SalaryCalculationService : ISalaryCalculationService
    {
        private readonly IPayrollStrategyFactory _payrollStrategyFactory;
        private readonly IChargeStrategyFactory _chargeStrategyFactory;

        public SalaryCalculationService(IPayrollStrategyFactory payrollStrategyFactory,
                                        IChargeStrategyFactory chargeStrategyFactory)
        {
            _payrollStrategyFactory = payrollStrategyFactory;
            _chargeStrategyFactory = chargeStrategyFactory;
        }

        public decimal GetSalary(int employeeId, DateTime forDate)
        {
            var payrollStrategy = _payrollStrategyFactory.GetStrategy(employeeId);
            var payroll = payrollStrategy.GetPayroll(employeeId, forDate);

            var chargeStrategy = _chargeStrategyFactory.GetStrategy(employeeId);
            var charge = chargeStrategy.GetCharge(employeeId, forDate);

            return payroll - charge;
        }
    }
}