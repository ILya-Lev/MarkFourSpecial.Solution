using Salary.DataAccess;
using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Linq;

namespace Salary.Services.Implementation.ChargeStrategies
{
    public class TradeUnionChargeStrategy : IChargeStrategy
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEntityForEmployeeRepository<ServiceCharge> _serviceChargeRepository;
        private readonly IEntityForEmployeeRepository<SalaryPayment> _salaryPaymentRepository;

        public TradeUnionChargeStrategy(IEmployeeRepository employeeRepository, IEntityForEmployeeRepository<ServiceCharge> serviceChargeRepository,
          IEntityForEmployeeRepository<SalaryPayment> salaryPaymentRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceChargeRepository = serviceChargeRepository;
            _salaryPaymentRepository = salaryPaymentRepository;
        }

        public decimal GetCharge(int employeeId, DateTime forDate)
        {
            var employee = _employeeRepository.Get(employeeId);
            if (!employee.TradeUnionCharge.HasValue)
            {
                throw new StrategyException($"{nameof(TradeUnionChargeStrategy)} is used for an employee {employeeId} which is not a trade union member");
            }

            var paymentsForMonth = _salaryPaymentRepository.GetForEmployee(employeeId, forDate.Subtract(TimeSpan.FromDays(31)), forDate);
            var lastPaymentDate = paymentsForMonth.OrderByDescending(p => p.Date).FirstOrDefault()?.Date;

            var daysSinceLastPayment = GetDaysSinceLastPayment(lastPaymentDate, forDate);
            var regularCharge = CalculateRegularCharge(employee, daysSinceLastPayment);

            var additionalCharges = _serviceChargeRepository.GetForEmployee(employeeId, lastPaymentDate, forDate);

            return regularCharge + additionalCharges.Sum(sc => sc.Amount);
        }

        private static int GetDaysSinceLastPayment(DateTime? lastPaymentDate, DateTime forDate)
        {
            return lastPaymentDate.HasValue
                ? forDate.Subtract(lastPaymentDate.Value).Days
                : forDate.Day;
        }

        private static decimal CalculateRegularCharge(Employee employee, int daysSinceLastPayment)
        {
            var weeksSoFar = daysSinceLastPayment / 7 + (daysSinceLastPayment % 7 == 0 ? 0 : 1);
            return employee.TradeUnionCharge.Value * weeksSoFar;
        }
    }
}
