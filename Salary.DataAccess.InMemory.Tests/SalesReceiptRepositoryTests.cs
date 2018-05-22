using FluentAssertions;
using Salary.Models;
using System;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class SalesReceiptRepositoryTests
    {
        private readonly InMemorySalesReceiptRepository _repository;

        public SalesReceiptRepositoryTests()
        {
            _repository = new InMemorySalesReceiptRepository();
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var salesReceipt = new SalesReceipt
            {
                Amount = 11m,
                Date = DateTime.Now,
                EmployeeId = 1972
            };

            var id = _repository.Create(salesReceipt);

            var storedReceipt = _repository.Get(id);

            storedReceipt.Amount.Should().Be(salesReceipt.Amount);
            storedReceipt.Date.Should().Be(salesReceipt.Date);
            storedReceipt.EmployeeId.Should().Be(salesReceipt.EmployeeId);
            storedReceipt.Id.Should().Be(id);
        }
    }
}
