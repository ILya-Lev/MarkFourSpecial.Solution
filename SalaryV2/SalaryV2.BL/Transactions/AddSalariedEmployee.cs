using SalaryV2.BL.Classification;
using SalaryV2.BL.Repositories;
using SalaryV2.BL.Schedule;

namespace SalaryV2.BL.Transactions
{
    public class AddSalariedEmployee : AddEmployeeTransaction
    {
        private readonly decimal _salary;

        public AddSalariedEmployee(IEmployeeRepository employeeRepository, string name, string address, decimal salary)
            : base(employeeRepository, name, address)
        {
            _salary = salary;
        }

        protected override IPaymentSchedule GetPaymentSchedule() => new MonthlySchedule();

        protected override IPaymentClassification GetPaymentClassification(int employeeId)
        {
            return new SalariedClassification(_salary);
        }
    }
}