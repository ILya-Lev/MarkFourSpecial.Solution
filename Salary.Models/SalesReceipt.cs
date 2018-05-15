using System;

namespace Salary.Models
{
    public class SalesReceipt
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
