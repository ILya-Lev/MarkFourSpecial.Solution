using System;

namespace Salary.Models
{
    public class EntityForEmployee : IEntityWithId
    {
        public EntityForEmployee(int employeeId)
        {
            EmployeeId = employeeId;
        }

        public int Id { get; set; }
        public int EmployeeId { get; }
        public DateTime Date { get; set; }
    }
}