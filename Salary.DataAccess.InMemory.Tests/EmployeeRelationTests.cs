using FluentAssertions;
using Salary.Models;
using Salary.Models.Errors;
using System.Net;
using Salary.DataAccess.Implementation;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class EmployeeRelationTests
    {
        private readonly EntityForEmployeeBaseRepository _baseRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly TimeCardRepository _timeCardRepository;
        private readonly SalesReceiptRepository _salesReceiptRepository;

        public EmployeeRelationTests()
        {
            _baseRepository = new EntityForEmployeeBaseRepository(new InMemoryStorage<EntityForEmployee>());
            _timeCardRepository = new TimeCardRepository(_baseRepository);
            _salesReceiptRepository = new SalesReceiptRepository(_baseRepository);
            _employeeRepository = new EmployeeRepository(_baseRepository, new InMemoryStorage<Employee>());
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
                _baseRepository.Get(tcId);
            }
            catch (RepositoryException exc)
            {
                exc.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
