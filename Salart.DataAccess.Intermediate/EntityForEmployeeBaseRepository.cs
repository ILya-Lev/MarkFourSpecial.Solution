using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

//[assembly: InternalsVisibleTo("Salary.DataAccess.InMemory.Tests")]

namespace Salary.DataAccess.Implementation
{
    public class EntityForEmployeeBaseRepository : IEntityForEmployeeBaseRepository, IDisposable
    {
        private readonly IStorage<EntityForEmployee> _storage;

        public EntityForEmployeeBaseRepository(IStorage<EntityForEmployee> storage)
        {
            _storage = storage;
        }

        public void Dispose()
        {
            _storage.Dispose();
        }

        public int Create<T>(T inMemoryInstance, Func<T, EntityForEmployee> cloner) where T : EntityForEmployee
        {
            if (inMemoryInstance == null) throw new ArgumentNullException(nameof(inMemoryInstance));
            if (cloner == null) throw new ArgumentNullException(nameof(cloner));
            lock (_storage)
            {
                var id = _storage.Entities.Count == 0 ? 1 : (_storage.Entities.Keys.Max() + 1);

                var clone = cloner(inMemoryInstance);
                clone.Id = id;
                _storage.Entities.Add(id, clone);

                return id;
            }
        }

        public T Delete<T>(int id) where T : EntityForEmployee
        {
            lock (_storage)
            {
                if (_storage.Entities.ContainsKey(id))
                {
                    var instance = _storage.Entities[id];
                    _storage.Entities.Remove(id);
                    return instance as T;
                }

                return default(T);
            }
        }

        public EntityForEmployee Get(int id)
        {
            if (_storage.Entities.ContainsKey(id))
                return _storage.Entities[id];

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
            var timeCards = _storage.Entities.Values.Where(val => val.EmployeeId == employeeId && predicate(val)).ToList();
            if (timeCards.Count != 0)
                return timeCards;

            throw new RepositoryException($"Cannot find any {typeof(EntityForEmployee).Name} with employee id '{employeeId}'{suffix}")
            {
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }
}