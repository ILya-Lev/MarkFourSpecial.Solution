﻿using System;
using System.Collections.Generic;
using System.Linq;
using Salary.Models;

namespace Salary.DataAccess.Implementation
{
    public class ServiceChargeRepository : IEntityForEmployeeRepository<ServiceCharge>
    {
        private readonly IEntityForEmployeeBaseRepository _repository;

        public ServiceChargeRepository(IEntityForEmployeeBaseRepository repository)
        {
            _repository = repository;
        }

        public int Create(ServiceCharge serviceCharge)
        {
            Func<ServiceCharge, EntityForEmployee> cloner = sc => new ServiceCharge(sc.EmployeeId)
            {
                Amount = sc.Amount,
                Date = sc.Date,
            };
            return _repository.Create(serviceCharge, cloner);
        }

        public ServiceCharge Get(int id)
        {
            return _repository.Get(id) as ServiceCharge;
        }

        public ICollection<ServiceCharge> GetForEmployee(int employeeId, DateTime? since = null, DateTime? until = null)
        {
            return _repository.GetForEmployee(employeeId, since, until).OfType<ServiceCharge>().ToList();
        }
    }
}
