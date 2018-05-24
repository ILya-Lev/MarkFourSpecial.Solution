using Salary.DataAccess;
using Salary.Models;
using System;

namespace Salary.Services.Implementation
{
    public class SalaryCalculationService : ISalaryCalculationService
    {
        private readonly IPayrollStrategyFactory _payrollStrategyFactory;
        private readonly IChargeStrategyFactory _chargeStrategyFactory;
        private readonly IEntityForEmployeeRepository<SalaryPayment> _salaryPaymentRepository;

        public SalaryCalculationService(IPayrollStrategyFactory payrollStrategyFactory,
                                        IChargeStrategyFactory chargeStrategyFactory,
                                        IEntityForEmployeeRepository<SalaryPayment> salaryPaymentRepository)
        {
            _payrollStrategyFactory = payrollStrategyFactory;
            _chargeStrategyFactory = chargeStrategyFactory;
            _salaryPaymentRepository = salaryPaymentRepository;
        }

        public decimal GetSalary(int employeeId, DateTime forDate)
        {
            var payrollStrategy = _payrollStrategyFactory.GetStrategy(employeeId);
            var payroll = payrollStrategy.GetPayroll(employeeId, forDate);

            //todo: think it over - business oriented decision - if employee is hourly payed but have done nothing
            // should we take the charge from him?
            if (payroll == 0m)
                return 0m;

            var chargeStrategy = _chargeStrategyFactory.GetStrategy(employeeId);
            var charge = chargeStrategy.GetCharge(employeeId, forDate);

            var payment = payroll - charge;
            _salaryPaymentRepository.Create(new SalaryPayment(employeeId)
            {
                Amount = payment,
                Date = DateTime.Today,
            });
            return payment;
        }
    }
}