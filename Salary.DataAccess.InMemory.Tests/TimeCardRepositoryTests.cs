using FluentAssertions;
using Salary.DataAccess.Intermediate;
using Salary.Models;
using System;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class TimeCardRepositoryTests
    {
        private readonly TimeCardRepository _repository;

        public TimeCardRepositoryTests()
        {
            _repository = new TimeCardRepository(new InMemoryEntityForEmployeeStorage());
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var timeCard = new TimeCard(1973)
            {
                Hours = 8.2f,
                Date = DateTime.Now,
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
