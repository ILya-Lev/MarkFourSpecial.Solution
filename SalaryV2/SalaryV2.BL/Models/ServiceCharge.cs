using System;

namespace SalaryV2.BL.Models
{
    public class ServiceCharge
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime At { get; set; }
        public decimal Amount { get; set; }
    }
}