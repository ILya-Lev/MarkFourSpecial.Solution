using FluentAssertions;
using Salary.DataAccess.Intermediate;
using Salary.Models;
using Salary.Models.Errors;
using System.Net;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class EmployeeRelationTests
    {
        private readonly InMemoryEntityForEmployeeStorage _storage;
        private readonly InMemoryEmployeeRepository _employeeRepository;
        private readonly TimeCardRepository _timeCardRepository;
        private readonly SalesReceiptRepository _salesReceiptRepository;

        public EmployeeRelationTests()
        {
            _storage = new InMemoryEntityForEmployeeStorage();
            _timeCardRepository = new TimeCardRepository(_storage);
            _salesReceiptRepository = new SalesReceiptRepository(_storage);
            _employeeRepository = new InMemoryEmployeeRepository(_storage);
        }

        [Fact]
        public void DeleteEmployee_ExistRelatedItems_CascadeDeleteRelatedItems()
        {
            var eId = _employeeRepository.Create(new Employee()
            {
                Name = "James",
                PaymentType = PaymentType.Hourly,
                MajorRate = 15m
            });

            var tcId = _timeCardRepository.Create(new TimeCard(eId) { Hours = 8.4f });
            var srId = _salesReceiptRepository.Create(new SalesReceipt(eId) { Amount = 75381m });

            tcId.Should().NotBe(srId).And.Be(srId - 1);

            _employeeRepository.Delete(eId);

            CheckEntityDeleted(tcId);
            CheckEntityDeleted(srId);
        }

        private void CheckEntityDeleted(int tcId)
        {
            try
            {
                _storage.Get(tcId);
            }
            catch (RepositoryException exc)
            {
                exc.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
