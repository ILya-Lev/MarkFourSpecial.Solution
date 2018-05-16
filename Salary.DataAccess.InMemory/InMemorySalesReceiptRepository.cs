using Salary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Salary.DataAccess.InMemory
{
    public class InMemorySalesReceiptRepository : IEntityForEmployeeRepository<SalesReceipt>
    {
        private readonly InMemoryEntityForEmployeeRepository _repository = new InMemoryEntityForEmployeeRepository();

        public int Create(SalesReceipt inMemoryInstance)
        {
            Func<SalesReceipt, EntityForEmployee> cloner = sr => new SalesReceipt
            {
                Date = sr.Date,
                EmployeeId = sr.EmployeeId,
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
            return _repository.GetForEmployee(employeeId, since, until).Cast<SalesReceipt>().ToList();
        }
    }
}