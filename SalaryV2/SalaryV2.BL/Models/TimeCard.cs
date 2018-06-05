using System;

namespace SalaryV2.BL.Models
{
    public class TimeCard
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime At { get; set; }
        public float Hours { get; set; }
    }
}