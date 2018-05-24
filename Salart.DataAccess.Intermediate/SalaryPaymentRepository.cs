using Salary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Salary.DataAccess.Implementation
{
    public class SalaryPaymentRepository : IEntityForEmployeeRepository<SalaryPayment>
    {
        private readonly IEntityForEmployeeBaseRepository _repository;

        public SalaryPaymentRepository(IEntityForEmployeeBaseRepository repository)
        {
            _repository = repository;
        }

        public int Create(SalaryPayment inMemoryPayment)
        {
            Func<SalaryPayment, EntityForEmployee> cloner = sp => new SalaryPayment(sp.EmployeeId)
            {
                Amount = sp.Amount,
                Date = sp.Date,
            };
            return _repository.Create(inMemoryPayment, cloner);
        }

        public SalaryPayment Get(int id)
        {
            return _repository.Get(id) as SalaryPayment;
        }

        public ICollection<SalaryPayment> GetForEmployee(int employeeId, DateTime? since, DateTime? until)
        {
            return _repository.GetForEmployee(employeeId, since, until).OfType<SalaryPayment>().ToList();
        }
    }
}