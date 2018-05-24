using FluentAssertions;
using Salary.DataAccess.Intermediate;
using Salary.Models;
using System;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class ServiceChargeRepositoryTests
    {
        private readonly ServiceChargeRepository _repository;

        public ServiceChargeRepositoryTests()
        {
            _repository = new ServiceChargeRepository(new InMemoryEntityForEmployeeStorage());
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var serviceCharge = new ServiceCharge(1972)
            {
                Amount = 11m,
                Date = DateTime.Now,
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
