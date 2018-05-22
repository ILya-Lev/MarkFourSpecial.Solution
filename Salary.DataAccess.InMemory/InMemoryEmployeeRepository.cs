using Salary.Models;
using Salary.Models.Errors;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Salary.DataAccess.InMemory
{
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly Dictionary<int, Employee> _storage = new Dictionary<int, Employee>();

        public int Create(Employee employee)
        {
            if (employee.Id != 0 && _storage.ContainsKey(employee.Id))
            {
                throw new RepositoryException($"Employee with id '{employee.Id}' already exists, cannot re-create.")
                {
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            if (employee.MajorRate == default(decimal))
            {
                throw new ValidationException("Major rate should be always specified!");
            }
            if (employee.PaymentType != PaymentType.Commissioned && employee.MinorRate.HasValue)
            {
                throw new ValidationException($"Employee with payment type '{employee.PaymentType}' should not have minor rate '{employee.MinorRate}'.");
            }
            if (employee.PaymentType == PaymentType.Commissioned && !employee.MinorRate.HasValue)
            {
                throw new ValidationException($"Employee with payment type '{employee.PaymentType}' should have minor rate.");
            }

            var id = _storage.Count == 0 ? 1 : (_storage.Keys.Max() + 1);
            var storedEmployee = new Employee
            {
                Id = id,
                Name = employee.Name,
                Address = employee.Address,
                PaymentType = employee.PaymentType,
                MajorRate = employee.MajorRate,
                MinorRate = employee.MinorRate,
                TradeUnionCharge = employee.TradeUnionCharge
            };
            _storage.Add(id, storedEmployee);
            return id;
        }

        public Employee Delete(int employeeId)
        {
            var employee = Get(employeeId);
            _storage.Remove(employeeId);
            return employee;
        }

        public Employee Get(int employeeId)
        {
            if (!_storage.ContainsKey(employeeId))
            {
                throw new RepositoryException($"Employee with id '{employeeId}' does not exist.")
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return _storage[employeeId];
        }

        public ICollection<Employee> GetAll()
        {
            return _storage.Values.OrderBy(employee => employee.Id).ToArray();
        }

        public Employee UpdateName(int employeeId, string name)
        {
            var employee = Get(employeeId);
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Employee name should be set");
            }
            employee.Name = name;

            return employee;
        }

        public Employee UpdateAddress(int employeeId, string address)
        {
            var employee = Get(employeeId);

            employee.Address = address;

            return employee;
        }

        public Employee UpdateHourly(int employeeId, decimal hourlyRate)
        {
            var employee = Get(employeeId);

            employee.PaymentType = PaymentType.Hourly;
            employee.MajorRate = hourlyRate;
            employee.MinorRate = null;

            return employee;
        }

        public Employee UpdateMonthly(int employeeId, decimal salary)
        {
            var employee = Get(employeeId);

            employee.PaymentType = PaymentType.Monthly;
            employee.MajorRate = salary;
            employee.MinorRate = null;

            return employee;
        }

        public Employee UpdateCommissioned(int employeeId, decimal salary, decimal rate)
        {
            var employee = Get(employeeId);

            employee.PaymentType = PaymentType.Commissioned;
            employee.MajorRate = salary;
            employee.MinorRate = rate;

            return employee;
        }

        public Employee UpdateTradeUnionCharge(int employeeId, decimal? charge)
        {
            var employee = Get(employeeId);

            employee.TradeUnionCharge = charge;

            return employee;
        }
    }
}