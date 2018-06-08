using SalaryV2.BL.Classification;
using SalaryV2.BL.Repositories;
using SalaryV2.BL.Schedule;

namespace SalaryV2.BL.Transactions
{
    public class AddHourlyEmployee : AddEmployeeTransaction
    {
        private readonly decimal _hourlyRate;

        public AddHourlyEmployee(IEmployeeRepository employeeRepository, string name, string address, decimal hourlyRate)
            : base(employeeRepository, name, address)
        {
            _hourlyRate = hourlyRate;
        }

        protected override IPaymentSchedule MakeSchedule() => new WeeklySchedule();

        protected override IPaymentClassification MakeClassification(int employeeId)
        {
            var timeCardProvider = new TimeCardProvider();
            return new HourlyClassification(employeeId, _hourlyRate, 1.5m, 2.5m, timeCardProvider);
        }
    }
}