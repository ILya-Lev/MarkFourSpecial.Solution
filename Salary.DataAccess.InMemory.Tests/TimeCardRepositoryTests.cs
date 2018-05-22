using FluentAssertions;
using Salary.Models;
using System;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class TimeCardRepositoryTests
    {
        private readonly InMemoryTimeCardRepository _repository;

        public TimeCardRepositoryTests()
        {
            _repository = new InMemoryTimeCardRepository();
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var timeCard = new TimeCard
            {
                Hours = 8.2f,
                Date = DateTime.Now,
                EmployeeId = 1972
            };

            var id = _repository.Create(timeCard);

            var storedReceipt = _repository.Get(id);

            storedReceipt.Hours.Should().Be(timeCard.Hours);
            storedReceipt.Date.Should().Be(timeCard.Date);
            storedReceipt.EmployeeId.Should().Be(timeCard.EmployeeId);
            storedReceipt.Id.Should().Be(id);
        }
    }
}
