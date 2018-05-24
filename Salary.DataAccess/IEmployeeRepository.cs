using Salary.Models;
using System;
using System.Collections.Generic;

namespace Salary.DataAccess
{
    public interface IEmployeeRepository : IDisposable
    {
        int Create(Employee employee);
        Employee Delete(int employeeId);

        Employee Get(int employeeId);
        ICollection<Employee> GetAll();

        Employee UpdateName(int employeeId, string name);
        Employee UpdateAddress(int employeeId, string address);
        Employee UpdateHourly(int employeeId, decimal hourlyRate);
        Employee UpdateMonthly(int employeeId, decimal salary);
        Employee UpdateCommissioned(int employeeId, decimal salary, decimal rate);
        Employee UpdateTradeUnionCharge(int employeeId, decimal? charge);
    }
}
