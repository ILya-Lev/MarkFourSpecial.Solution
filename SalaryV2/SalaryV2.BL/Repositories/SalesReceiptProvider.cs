using SalaryV2.BL.Models;
using System;
using System.Collections.Generic;

namespace SalaryV2.BL.Repositories
{
    public class SalesReceiptProvider : IEmployeeRelatedProvider<SalesReceipt>, IEmployeeRelatedKeeper<SalesReceipt>
    {
        private static readonly Dictionary<int, SalesReceipt> _storage = new Dictionary<int, SalesReceipt>();

        private static readonly EmployeeRelatedAlgorithms<SalesReceipt> _algorithm = new EmployeeRelatedAlgorithms<SalesReceipt>();

        public ICollection<SalesReceipt> GetForEmployee(int employeeId, DateTime? since, DateTime? until)
        {
            return _algorithm.GetForEmployee(_storage, employeeId, since, until);
        }

        public SalesReceipt Get(int id)
        {
            return _algorithm.Get(_storage, id);
        }

        public int Add(SalesReceipt entity)
        {
            return _algorithm.Add(_storage, entity);
        }
    }
}