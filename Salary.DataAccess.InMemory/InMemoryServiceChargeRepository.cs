using Salary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Salary.DataAccess.InMemory
{
    public class InMemoryServiceChargeRepository : IEntityForEmployeeRepository<ServiceCharge>
    {
        private readonly InMemoryEntityForEmployeeRepository _repository = new InMemoryEntityForEmployeeRepository();

        public int Create(ServiceCharge serviceCharge)
        {
            Func<ServiceCharge, EntityForEmployee> cloner = sc => new ServiceCharge
            {
                Amount = sc.Amount,
                Date = sc.Date,
                EmployeeId = sc.EmployeeId
            };
            return _repository.Create(serviceCharge, cloner);
        }

        public ServiceCharge Get(int id)
        {
            return _repository.Get(id) as ServiceCharge;
        }

        public ICollection<ServiceCharge> GetForEmployee(int employeeId, DateTime? since = null, DateTime? until = null)
        {
            return _repository.GetForEmployee(employeeId, since, until).Cast<ServiceCharge>().ToList();
        }
    }
}
