using FluentAssertions;
using Salary.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Salary.DataAccess.Implementation;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class SalaryPaymentRepositoryTests
    {
        private readonly SalaryPaymentRepository _repository;

        public SalaryPaymentRepositoryTests()
        {
            _repository = new SalaryPaymentRepository(new EntityForEmployeeBaseRepository(new InMemoryStorage<EntityForEmployee>()));
        }

        [Fact]
        public void Create_CopiesAllProperties()
        {
            var salaryPayment = new SalaryPayment(11)
            {
                Amount = 10m,
                Date = DateTime.Now,
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
            var salaryPayment = new SalaryPayment(11)
            {
                Amount = 10m,
                Date = DateTime.Now,
            };
            var itemsCount = 1000;

            Parallel.For(0, itemsCount, idx => _repository.Create(salaryPayment));

            var payments = Enumerable.Range(0, itemsCount).Select(idx => _repository.Get(idx + 1)).Distinct().ToList();
            payments.Should().HaveCount(itemsCount);
        }
    }
}
