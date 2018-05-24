using Salary.Models;
using System;
using System.Collections.Generic;

namespace Salary.DataAccess.Implementation
{
    public interface IEntityForEmployeeBaseRepository
    {
        int Create<T>(T inMemoryInstance, Func<T, EntityForEmployee> cloner) where T : EntityForEmployee;
        T Delete<T>(int id) where T : EntityForEmployee;
        EntityForEmployee Get(int id);
        ICollection<EntityForEmployee> GetForEmployee(int employeeId, DateTime? since = null, DateTime? until = null);
    }
}