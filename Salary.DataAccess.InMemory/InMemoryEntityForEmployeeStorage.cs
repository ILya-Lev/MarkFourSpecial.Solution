using Salary.DataAccess.Intermediate;
using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

//[assembly: InternalsVisibleTo("Salary.DataAccess.InMemory.Tests")]

namespace Salary.DataAccess.InMemory
{
    public class InMemoryEntityForEmployeeStorage : IEntityForEmployeeStorage
    {
        private readonly Dictionary<int, EntityForEmployee> _storage = new Dictionary<int, EntityForEmployee>();

        public int Create<T>(T inMemoryInstance, Func<T, EntityForEmployee> cloner) where T : EntityForEmployee
        {
            if (inMemoryInstance == null) throw new ArgumentNullException(nameof(inMemoryInstance));
            if (cloner == null) throw new ArgumentNullException(nameof(cloner));
            lock (_storage)
            {
                var id = _storage.Count == 0 ? 1 : (_storage.Keys.Max() + 1);

                var clone = cloner(inMemoryInstance);
                clone.Id = id;
                _storage.Add(id, clone);

                return id;
            }
        }

        public T Delete<T>(int id) where T : EntityForEmployee
        {
            lock (_storage)
            {
                if (_storage.ContainsKey(id))
                {
                    var instance = _storage[id];
                    _storage.Remove(id);
                    return instance as T;
                }

                return default(T);
            }
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
            if (timeCards.Count != 0)
                return timeCards;

            throw new RepositoryException($"Cannot find any {typeof(EntityForEmployee).Name} with employee id '{employeeId}'{suffix}")
            {
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }
}