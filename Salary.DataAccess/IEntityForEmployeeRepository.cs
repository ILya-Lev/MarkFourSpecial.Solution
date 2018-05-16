using Salary.Models;
using System;
using System.Collections.Generic;

namespace Salary.DataAccess
{
    public interface IEntityForEmployeeRepository<T> where T : EntityForEmployee
    {
        int Create(T inMemoryPayment);
        T Get(int id);
        ICollection<T> GetForEmployee(int employeeId, DateTime? since = null, DateTime? until = null);
    }
}