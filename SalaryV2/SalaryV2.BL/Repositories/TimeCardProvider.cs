using SalaryV2.BL.Models;
using System;
using System.Collections.Generic;

namespace SalaryV2.BL.Repositories
{
    /// <summary>
    /// mono-state pattern
    /// </summary>
    public class TimeCardProvider : IEmployeeRelatedProvider<TimeCard>, IEmployeeRelatedKeeper<TimeCard>
    {
        private static readonly Dictionary<int, TimeCard> _storage = new Dictionary<int, TimeCard>();
        private static readonly EmployeeRelatedAlgorithms<TimeCard> _algorithms = new EmployeeRelatedAlgorithms<TimeCard>();
        public ICollection<TimeCard> GetForEmployee(int employeeId, DateTime? since, DateTime? until)
        {
            return _algorithms.GetForEmployee(_storage, employeeId, since, until);
        }

        public TimeCard Get(int id)
        {
            return _algorithms.Get(_storage, id);
        }

        public int Add(TimeCard entity)
        {
            return _algorithms.Add(_storage, entity);
        }
    }
}