﻿using FluentAssertions;
using Salary.Models;
using System;
using Salary.DataAccess.Implementation;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class SalesReceiptRepositoryTests
    {
        private readonly SalesReceiptRepository _repository;

        public SalesReceiptRepositoryTests()
        {
            _repository = new SalesReceiptRepository(new EntityForEmployeeBaseRepository(new InMemoryStorage<EntityForEmployee>()));
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var salesReceipt = new SalesReceipt(1973)
            {
                Amount = 11m,
                Date = DateTime.Now,
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
