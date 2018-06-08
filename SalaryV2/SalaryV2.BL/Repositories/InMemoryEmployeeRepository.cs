using SalaryV2.BL.Models;
using System.Collections.Generic;
using System.Linq;

namespace SalaryV2.BL.Repositories
{
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly Dictionary<int, Employee> _storage = new Dictionary<int, Employee>();

        public int Create(Employee employee)
        {
            lock (_storage)
            {
                var id = _storage.Count == 0 ? 1 : (_storage.Keys.Max() + 1);
                _storage.Add(id, employee);
                return id;
            }
        }
    }
}