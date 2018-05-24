using System;
using System.Collections.Generic;
using System.Linq;
using Salary.Models;

namespace Salary.DataAccess.Implementation
{
    public class TimeCardRepository : IEntityForEmployeeRepository<TimeCard>
    {
        private readonly IEntityForEmployeeBaseRepository _repository;

        public TimeCardRepository(IEntityForEmployeeBaseRepository repository)
        {
            _repository = repository;
        }

        public int Create(TimeCard inMemoryInstance)
        {
            Func<TimeCard, EntityForEmployee> cloner = tc => new TimeCard(tc.EmployeeId)
            {
                Date = tc.Date,
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
            return _repository.GetForEmployee(employeeId, since, until).OfType<TimeCard>().ToList();
        }
    }
}
