using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Salary.DataAccess.InMemory
{
    internal class InMemoryEntityForEmployeeRepository
    {
        private readonly Dictionary<int, EntityForEmployee> _storage = new Dictionary<int, EntityForEmployee>();

        public int Create<T>(T inMemoryInstance, Func<T, EntityForEmployee> cloner) where T : EntityForEmployee
        {
            var id = _storage.Count == 0 ? 1 : (_storage.Keys.Max() + 1);

            var clone = cloner(inMemoryInstance);
            clone.Id = id;
            _storage.Add(id, clone);

            return id;
        }

        public EntityForEmployee Get(int id)
        {
            if (_storage.ContainsKey(id))
                return _storage[id];

            throw new RepositoryException($"Cannot find {typeof(EntityForEmployee).Name} with id '{id}'.")
            {
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public ICollection<EntityForEmployee> GetForEmployee(int employeeId, DateTime? since = null, DateTime? until = null)
        {
            if (since == null && until == null)
            {
                return GetBy(employeeId, e => true, "");
            }

            if (until == null)
            {
                return GetBy(employeeId, e => e.Date > since, $" after '{since:g}'");
            }

            if (since == null)
            {
                return GetBy(employeeId, e => e.Date <= until, $" before '{until:g}'");
            }

            return GetBy(employeeId, e => e.Date > since && e.Date <= until, $" after '{since:g}' and before '{until: g}'");
        }

        private ICollection<EntityForEmployee> GetBy(int employeeId, Func<EntityForEmployee, bool> predicate, string suffix)
        {
            var timeCards = _storage.Values.Where(val => val.EmployeeId == employeeId && predicate(val)).ToList();
            if (timeCards.Count == 0)
                throw new RepositoryException($"Cannot find any time card with employee id '{employeeId}'{suffix}")
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            return timeCards;
        }
    }
}