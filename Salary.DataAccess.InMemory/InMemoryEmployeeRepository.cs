using Salary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Salary.DataAccess.InMemory
{
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly Dictionary<int, Employee> _storage = new Dictionary<int, Employee>();

        public int Create(Employee inMemoryInstance)
        {
            if (inMemoryInstance.Id != 0 && _storage.ContainsKey(inMemoryInstance.Id))
            {
                throw new RepositoryException(
                    $"Employee with id '{inMemoryInstance.Id}' already exists, cannot re-create.");
            }

            var id = _storage.Keys.Max() + 1;
            var storedEmployee = new Employee(inMemoryInstance) { Id = id };
            _storage.Add(id, storedEmployee);
            return id;
        }

        public Employee Get(int employeeId)
        {
            if (!_storage.ContainsKey(employeeId))
            {
                throw new RepositoryException($"Employee with id '{employeeId}' does not exist.");
            }

            return _storage[employeeId];
        }


        public Employee Update(Employee editedEmployee)
        {
            var oldEmployee = Get(editedEmployee.Id);
            //todo how to do it via reflection? or just introduce a bunch of update methods
            //for each case
            var props = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var value = prop.GetValue(editedEmployee);
                prop.SetValue(oldEmployee, value);
            }
        }

        public Employee Delete(int employeeId)
        {
            var anEmployee = Get(employeeId);
            _storage.Remove(employeeId);
            return anEmployee;
        }
    }
}