namespace Salary.Models
{
    public class ServiceCharge : EntityForEmployee
    {
        public decimal Amount { get; set; }

        public ServiceCharge(int employeeId) : base(employeeId) { }
    }
}