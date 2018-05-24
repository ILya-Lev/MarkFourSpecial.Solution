using FluentAssertions;
using Salary.DataAccess.Intermediate;
using Salary.Models;
using Salary.Models.Errors;
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

        [InlineData(0)]
        [InlineData(0.5f)]
        [InlineData(-1)]
        [InlineData(24.1f)]
        [InlineData(30)]
        [Theory]
        public void Create_InvalidHours_ThrowsException(float hours)
        {
            var timeCard = new TimeCard(1973) { Date = DateTime.Now };

            Action creation = () => timeCard.Hours = hours;

            creation.Should().Throw<ValidationException>();
        }
    }
}
