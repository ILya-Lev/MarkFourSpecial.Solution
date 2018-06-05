using SalaryV2.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalaryV2.BL.Classification
{
    public class CommissionedClassification : IPaymentClassification
    {
        private readonly decimal _commissionRate;
        private readonly decimal _salary;
        private readonly int _employeeId;
        private readonly IEmployeeRelatedProvider<SalesReceipt> _salesReceiptProvider;

        public CommissionedClassification(decimal commissionRate, decimal salary, int employeeId,
                                            IEmployeeRelatedProvider<SalesReceipt> salesReceiptProvider)
        {
            _commissionRate = commissionRate;
            _salary = salary;
            _employeeId = employeeId;
            _salesReceiptProvider = salesReceiptProvider;
        }

        public decimal Pay(DateTime aDate, DateTime? lastPayday)
        {
            var salesReceipts = GetSalesReceiptsForEmployee(aDate, lastPayday);
            var overallSales = salesReceipts.Sum(sr => sr.Amount);

            return overallSales * _commissionRate + _salary;
        }

        private ICollection<SalesReceipt> GetSalesReceiptsForEmployee(DateTime aDate, DateTime? lastPayday)
        {
            return _salesReceiptProvider.GetForEmployee(_employeeId, lastPayday, aDate);
        }

        //private DateTime LastPaymentDate(DateTime aDate)
        //{
        //    var year = aDate.Month == 1 ? aDate.Year - 1 : aDate.Year;
        //    var month = aDate.Month == 1 ? 12 : (aDate.Month - 1);
        //    var previousMonthFirstDay = new DateTime(year, month, 1);
        //    for (int i = 1; i < 31; i++)
        //    {
        //        var day = previousMonthFirstDay.AddDays(i);
        //        if (IsPayDate(day))
        //            return day;
        //    }
        //    return aDate;
        //}
    }
}