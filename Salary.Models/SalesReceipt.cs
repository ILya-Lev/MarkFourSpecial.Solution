namespace Salary.Models
{
    public class SalesReceipt : EntityForEmployee
    {
        public decimal Amount { get; set; }

        public SalesReceipt(int employeeId) : base(employeeId) { }
    }
}
