using SalaryV2.BL.Transactions;
using System;

namespace SalaryV2.BL.Models
{
    public class SalesReceipt : EntityForEmployee
    {
        public SalesReceipt(int employeeId, DateTime? at = null) : base(employeeId, at)
        {
        }

        public decimal Amount { get; set; }
    }
}