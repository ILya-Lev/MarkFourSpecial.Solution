using SalaryV2.BL.Classification;
using SalaryV2.BL.Repositories;
using SalaryV2.BL.Schedule;

namespace SalaryV2.BL.Transactions
{
    public class AddCommissionedEmployee : AddEmployeeTransaction
    {
        private readonly decimal _salary;
        private readonly decimal _commissionedRate;

        public AddCommissionedEmployee(IEmployeeRepository employeeRepository, string name, string address, decimal salary, decimal commissionedRate)
            : base(employeeRepository, name, address)
        {
            _salary = salary;
            _commissionedRate = commissionedRate;
        }

        protected override IPaymentSchedule MakeSchedule() => new BiweeklySchedule();

        protected override IPaymentClassification MakeClassification(int employeeId)
        {
            var salesReceiptProvider = new SalesReceiptProvider();
            return new CommissionedClassification(_commissionedRate, _salary, employeeId, salesReceiptProvider);
        }
    }
}