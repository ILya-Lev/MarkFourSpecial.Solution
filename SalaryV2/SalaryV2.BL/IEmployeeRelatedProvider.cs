using SalaryV2.BL.Transactions;
using System;
using System.Collections.Generic;
using SalaryV2.BL.Models;

namespace SalaryV2.BL
{
    public interface IEmployeeRelatedProvider<T> where T : EntityForEmployee
    {
        ICollection<T> GetForEmployee(int employeeId, DateTime? since, DateTime? until);
        T Get(int id);
    }
}