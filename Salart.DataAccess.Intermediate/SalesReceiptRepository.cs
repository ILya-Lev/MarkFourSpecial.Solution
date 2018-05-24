using System;
using System.Collections.Generic;
using System.Linq;
using Salary.Models;

namespace Salary.DataAccess.Implementation
{
    public class SalesReceiptRepository : IEntityForEmployeeRepository<SalesReceipt>
    {
        private readonly IEntityForEmployeeBaseRepository _repository;

        public SalesReceiptRepository(IEntityForEmployeeBaseRepository repository)
        {
            _repository = repository;
        }

        public int Create(SalesReceipt inMemoryInstance)
        {
            Func<SalesReceipt, EntityForEmployee> cloner = sr => new SalesReceipt(sr.EmployeeId)
            {
                Date = sr.Date,
                Amount = sr.Amount
            };
            return _repository.Create(inMemoryInstance, cloner);
        }

        public SalesReceipt Get(int id)
        {
            return _repository.Get(id) as SalesReceipt;
        }

        public ICollection<SalesReceipt> GetForEmployee(int employeeId, DateTime? since = null, DateTime? until = null)
        {
            return _repository.GetForEmployee(employeeId, since, until).OfType<SalesReceipt>().ToList();
        }
    }
}