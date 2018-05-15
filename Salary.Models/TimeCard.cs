using System;

namespace Salary.Models
{
    public class TimeCard
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public float Hours { get; set; }
    }
}