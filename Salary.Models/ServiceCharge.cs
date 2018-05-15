namespace Salary.Models
{
    public class ServiceCharge
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
    }
}