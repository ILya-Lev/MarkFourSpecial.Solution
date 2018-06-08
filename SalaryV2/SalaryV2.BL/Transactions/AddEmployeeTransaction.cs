using SalaryV2.BL.Classification;
using SalaryV2.BL.Models;
using SalaryV2.BL.Repositories;
using SalaryV2.BL.Schedule;

namespace SalaryV2.BL.Transactions
{
    public abstract class AddEmployeeTransaction : ITransaction
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly string _name;
        private readonly string _address;

        public AddEmployeeTransaction(IEmployeeRepository employeeRepository, string name, string address)
        {
            _employeeRepository = employeeRepository;
            _name = name;
            _address = address;
        }

        public int Execute()
        {
            var employee = new Employee
            {
                Name = _name,
                Address = _address,
                PaymentMethod = new HoldMethod(),
                PaymentSchedule = MakeSchedule()
            };
            //todo: Affiliations ?
            var id = _employeeRepository.Create(employee);
            employee.PaymentClassification = MakeClassification(id);
            return id;
        }

        protected abstract IPaymentSchedule MakeSchedule();

        protected abstract IPaymentClassification MakeClassification(int employeeId);
    }
}