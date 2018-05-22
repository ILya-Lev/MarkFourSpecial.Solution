﻿using FluentAssertions;
using Salary.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class SalaryPaymentRepositoryTests
    {
        private readonly InMemorySalaryPaymentRepository _repository;

        public SalaryPaymentRepositoryTests()
        {
            _repository = new InMemorySalaryPaymentRepository();
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var salaryPayment = new SalaryPayment
            {
                Amount = 10m,
                Date = DateTime.Now,
                EmployeeId = 11
            };
            var id = _repository.Create(salaryPayment);

            var storedPayment = _repository.Get(id);

            storedPayment.Amount.Should().Be(salaryPayment.Amount);
            storedPayment.Date.Should().Be(salaryPayment.Date);
            storedPayment.EmployeeId.Should().Be(salaryPayment.EmployeeId);
            storedPayment.Id.Should().Be(id);
        }

        [Fact]
        public void Create_FromManyThreads_GeneratesUniqueIds()
        {
            var salaryPayment = new SalaryPayment
            {
                Amount = 10m,
                Date = DateTime.Now,
                EmployeeId = 11
            };
            var itemsCount = 1000;

            Parallel.For(0, itemsCount, idx => _repository.Create(salaryPayment));

            var payments = Enumerable.Range(0, itemsCount).Select(idx => _repository.Get(idx + 1)).Distinct().ToList();
            payments.Should().HaveCount(itemsCount);
        }
    }
}
