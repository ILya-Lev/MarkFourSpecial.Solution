﻿using Salary.DataAccess;
using System;

namespace Salary.Services.Implementation.ChargeStrategies
{
    public class NoneChargeStrategy : IChargeStrategy
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

            throw new Exception($"{nameof(NoneChargeStrategy)} is used for an employee {employeeId} which is a trade union member");
        }
    }
}