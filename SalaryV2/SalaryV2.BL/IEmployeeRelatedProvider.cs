using System;
using System.Collections.Generic;

namespace SalaryV2.BL
{
    public interface IEmployeeRelatedProvider<T>
    {
        ICollection<T> GetForEmployee(int employeeId, DateTime? since, DateTime? until);
        T Get(int id);
    }
}