using System;
using System.Linq;
using SalaryV2.BL.Models;

namespace SalaryV2.BL.Affiliation
{
    public class UnionAffiliation : IAffiliation
    {
        private readonly decimal _dues;
        private readonly int _employeeId;
        private readonly IEmployeeRelatedProvider<ServiceCharge> _serviceChargeProvider;

        public UnionAffiliation(decimal dues, int employeeId, IEmployeeRelatedProvider<ServiceCharge> serviceChargeProvider)
        {
            _dues = dues;
            _employeeId = employeeId;
            _serviceChargeProvider = serviceChargeProvider;
        }

        public decimal GetCharge(DateTime since, DateTime until)
        {
            var serviceCharges = _serviceChargeProvider.GetForEmployee(_employeeId, since, until);
            var totalCharges = serviceCharges.Sum(sc => sc.Amount);
            //todo: should be taken each time we call this method, or not
            return _dues + totalCharges;
        }
    }
}