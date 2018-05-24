namespace Salary.Models
{
    public class SalaryPayment : EntityForEmployee
    {
        public decimal Amount { get; set; }

        public SalaryPayment(int employeeId) : base(employeeId) { }
    }
}