using System;

namespace SalaryV2.BL.Models
{
    public class EntityForEmployee
    {
        public EntityForEmployee(int employeeId, DateTime? at = null)
        {
            EmployeeId = employeeId;
            At = at ?? DateTime.Now;
        }

        public int Id { get; set; }
        public int EmployeeId { get; }
        public DateTime At { get; }
    }
}