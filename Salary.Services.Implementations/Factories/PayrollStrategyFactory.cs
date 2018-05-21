using Salary.DataAccess;
using Salary.Models;
using Salary.Models.Errors;
using System.Collections.Generic;

namespace Salary.Services.Implementation.Factories
{
    public class PayrollStrategyFactory : IPayrollStrategyFactory
    {
        private readonly Dictionary<PaymentType, IPayrollStrategy> _supportedStrategies;
        private readonly IEmployeeRepository _employeeRepository;

        public PayrollStrategyFactory(IHourlyPayrollStrategy hourly, IMonthlyPayrollStrategy monthly, ICommissionedPayrollStrategy commission, IEmployeeRepository employeeRepository)
        {
            _supportedStrategies = new Dictionary<PaymentType, IPayrollStrategy>
            {
                [hourly.PaymentType] = hourly,
                [monthly.PaymentType] = monthly,
                [commission.PaymentType] = commission
            };
            _employeeRepository = employeeRepository;
        }

        public IPayrollStrategy GetStrategy(int employeeId)
        {
            var employee = _employeeRepository.Get(employeeId);
            if (_supportedStrategies.ContainsKey(employee.PaymentType))
            {
                return _supportedStrategies[employee.PaymentType];
            }

            throw new StrategyException($"Missing payroll strategy for payment type {employee.PaymentType}");
        }
    }
}
