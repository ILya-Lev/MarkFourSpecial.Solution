using SalaryV2.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalaryV2.BL.Repositories
{
    public class EmployeeRelatedAlgorithms<T> where T : EntityForEmployee
    {

        public ICollection<T> GetForEmployee(Dictionary<int, T> storage, int employeeId, DateTime? since, DateTime? until)
        {
            return storage.Select(pair => pair.Value)
                .Where(item => item.EmployeeId == employeeId)
                .Where(item => item.At >= (since ?? item.At) && item.At <= (until ?? item.At))
                .ToList();
        }

        public T Get(Dictionary<int, T> storage, int id)
        {
            return storage.ContainsKey(id) ? storage[id] : default(T);
        }

        public int Add(Dictionary<int, T> storage, T entity)
        {
            lock (storage)
            {
                var id = storage.Count == 0 ? 1 : (storage.Keys.Max() + 1);
                storage.Add(id, entity);
                return id;
            }
        }
    }
}