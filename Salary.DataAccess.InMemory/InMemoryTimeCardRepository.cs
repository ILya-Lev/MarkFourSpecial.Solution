using Salary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Salary.DataAccess.InMemory
{
    public class InMemoryTimeCardRepository : IEntityForEmployeeRepository<TimeCard>
    {
        private readonly InMemoryEntityForEmployeeRepository _repository = new InMemoryEntityForEmployeeRepository();

        public int Create(TimeCard inMemoryInstance)
        {
            Func<TimeCard, EntityForEmployee> cloner = tc => new TimeCard
            {
                Date = tc.Date,
                EmployeeId = tc.EmployeeId,
                Hours = tc.Hours
            };
            return _repository.Create(inMemoryInstance, cloner);
        }

        public TimeCard Get(int id)
        {
            return _repository.Get(id) as TimeCard;
        }

        public ICollection<TimeCard> GetForEmployee(int employeeId, DateTime? since = null, DateTime? until = null)
        {
            return _repository.GetForEmployee(employeeId, since, until).Cast<TimeCard>().ToList();
        }
    }
}
