using Salary.DataAccess;
using Salary.Models.Errors;
using System;

namespace Salary.Services.Implementation.ChargeStrategies
{
    public class NoneChargeStrategy : INoneChargeStrategy
    {
        private readonly IEmployeeRepository _employeeRepository;

        public NoneChargeStrategy(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public decimal GetCharge(int employeeId, DateTime forDate)
        {
            var employee = _employeeRepository.Get(employeeId);
            if (!employee.TradeUnionCharge.HasValue)
                return 0m;

            throw new StrategyException($"{nameof(NoneChargeStrategy)} is used for an employee {employeeId} which is a trade union member");
        }
    }
}