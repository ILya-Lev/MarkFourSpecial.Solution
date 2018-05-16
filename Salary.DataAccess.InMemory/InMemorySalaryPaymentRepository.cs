using Salary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Salary.DataAccess.InMemory
{
    public class InMemorySalaryPaymentRepository : IEntityForEmployeeRepository<SalaryPayment>
    {
        private readonly InMemoryEntityForEmployeeRepository _repository = new InMemoryEntityForEmployeeRepository();

        public int Create(SalaryPayment inMemoryPayment)
        {
            Func<SalaryPayment, EntityForEmployee> cloner = sp => new SalaryPayment
            {
                Amount = sp.Amount,
                Date = sp.Date,
                EmployeeId = sp.EmployeeId
            };
            return _repository.Create(inMemoryPayment, cloner);
        }

        public SalaryPayment Get(int id)
        {
            return _repository.Get(id) as SalaryPayment;
        }

        public ICollection<SalaryPayment> GetForEmployee(int employeeId, DateTime? since, DateTime? until)
        {
            return _repository.GetForEmployee(employeeId, since, until).Cast<SalaryPayment>().ToList();
        }
    }
}