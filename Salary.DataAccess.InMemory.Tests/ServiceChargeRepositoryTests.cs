using FluentAssertions;
using Salary.Models;
using System;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class ServiceChargeRepositoryTests
    {
        private readonly InMemoryServiceChargeRepository _repository;

        public ServiceChargeRepositoryTests()
        {
            _repository = new InMemoryServiceChargeRepository();
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var serviceCharge = new ServiceCharge
            {
                Amount = 11m,
                Date = DateTime.Now,
                EmployeeId = 1972
            };

            var id = _repository.Create(serviceCharge);

            var storedReceipt = _repository.Get(id);

            storedReceipt.Amount.Should().Be(serviceCharge.Amount);
            storedReceipt.Date.Should().Be(serviceCharge.Date);
            storedReceipt.EmployeeId.Should().Be(serviceCharge.EmployeeId);
            storedReceipt.Id.Should().Be(id);
        }
    }
}
