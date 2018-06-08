using System;

namespace SalaryV2.BL.Models
{
    public class TimeCard : EntityForEmployee
    {
        public TimeCard(int employeeId, DateTime? at = null) : base(employeeId, at)
        {
        }

        public float Hours { get; set; }
    }
}