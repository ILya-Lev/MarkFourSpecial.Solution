using Salary.Models;
using System.Collections.Generic;
using System.Linq;

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

        public Employee Delete(int employeeId)
        {
            var anEmployee = Get(employeeId);
            _storage.Remove(employeeId);
            return anEmployee;
        }

        public Employee Get(int employeeId)
        {
            if (!_storage.ContainsKey(employeeId))
            {
                throw new RepositoryException($"Employee with id '{employeeId}' does not exist.");
            }

            return _storage[employeeId];
        }

        public Employee UpdateName(int employeeId, string name)
        {
            var oldEmployee = Get(employeeId);

            oldEmployee.Name = name;

            return oldEmployee;
        }

        public Employee UpdateAddress(int employeeId, string address)
        {
            var oldEmployee = Get(employeeId);

            oldEmployee.Address = address;

            return oldEmployee;
        }

        public Employee UpdateHourly(int employeeId, decimal hourlyRate)
        {
            var oldEmployee = Get(employeeId);

            oldEmployee.PaymentType = PaymentType.Hourly;
            oldEmployee.MajorRate = hourlyRate;
            oldEmployee.MinorRate = null;

            return oldEmployee;
        }

        public Employee UpdateSalaried(int employeeId, decimal salary)
        {
            var oldEmployee = Get(employeeId);

            oldEmployee.PaymentType = PaymentType.Salary;
            oldEmployee.MajorRate = salary;
            oldEmployee.MinorRate = null;

            return oldEmployee;
        }

        public Employee UpdateCommissioned(int employeeId, decimal salary, decimal rate)
        {
            var oldEmployee = Get(employeeId);

            oldEmployee.PaymentType = PaymentType.Commissioned;
            oldEmployee.MajorRate = salary;
            oldEmployee.MinorRate = rate;

            return oldEmployee;
        }
    }
}