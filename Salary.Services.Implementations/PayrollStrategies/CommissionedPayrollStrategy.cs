using Salary.DataAccess;
using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Linq;

namespace Salary.Services.Implementation.PayrollStrategies
{
    public class CommissionedPayrollStrategy : MonthlyPayrollStrategy, ICommissionedPayrollStrategy
    {
        private readonly IEntityForEmployeeRepository<SalesReceipt> _salesReceiptRepository;

        public CommissionedPayrollStrategy(IEmployeeRepository employeeRepository, IEntityForEmployeeRepository<SalesReceipt> salesReceiptRepository)
            : base(employeeRepository)
        {
            _salesReceiptRepository = salesReceiptRepository;
        }

        public override PaymentType PaymentType => PaymentType.Commissioned;

        public override decimal GetPayroll(int employeeId, DateTime forDate)
        {
            if (!IsSecondFriday(forDate))
                return base.GetPayroll(employeeId, forDate);

            var employee = _employeeRepository.Get(employeeId);

            var salesInTwoWeeks = SalesInTwoWeeks(employeeId, forDate);
            var commission = salesInTwoWeeks * employee.MinorRate.Value;
            return commission + base.GetPayroll(employeeId, forDate);
        }

        private decimal SalesInTwoWeeks(int employeeId, DateTime forDate)
        {
            try
            {
                var previousSecondFriday = GetPreviousSecondFriday(forDate);
                var sales = _salesReceiptRepository.GetForEmployee(employeeId, previousSecondFriday, forDate);
                return sales.Sum(s => s.Amount);
            }
            catch (RepositoryException exc)
            {
                return 0m;
            }
        }

        private bool IsSecondFriday(DateTime forDate)
        {
            if (forDate.DayOfWeek != DayOfWeek.Friday)
                return false;

            if (WasPreviousMonthWeeksAgo(forDate, 1)) return false;

            if (WasPreviousMonthWeeksAgo(forDate, 2)) return true;

            if (WasPreviousMonthWeeksAgo(forDate, 3)) return false;

            if (WasPreviousMonthWeeksAgo(forDate, 4)) return true;

            return false;
        }

        private static bool WasPreviousMonthWeeksAgo(DateTime forDate, int weeksAmount)
        {
            var weeksAgo = forDate.Subtract(TimeSpan.FromDays(7 * weeksAmount));
            return weeksAgo.Month == forDate.Month - 1 || weeksAgo.Year == forDate.Year - 1;
        }

        private DateTime GetPreviousSecondFriday(DateTime forDate)
        {
            var candidate = forDate.Subtract(TimeSpan.FromDays(7 * 2));
            while (candidate.DayOfWeek != DayOfWeek.Friday)
            {
                candidate = candidate.Subtract(TimeSpan.FromDays(1));
            }
            return candidate;
        }
    }
}