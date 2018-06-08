using System;

namespace SalaryV2.BL.Models
{
    public class ServiceCharge : EntityForEmployee
    {
        public ServiceCharge(int employeeId, DateTime? at = null) : base(employeeId, at)
        {
        }

        public decimal Amount { get; set; }
    }
}