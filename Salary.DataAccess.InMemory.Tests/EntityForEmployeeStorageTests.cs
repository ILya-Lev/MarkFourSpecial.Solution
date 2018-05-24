using FluentAssertions;
using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Net;
using Salary.DataAccess.Implementation;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class EntityForEmployeeStorageTests
    {
        private readonly EntityForEmployeeBaseRepository _baseRepository;

        public EntityForEmployeeStorageTests()
        {
            _baseRepository = new EntityForEmployeeBaseRepository(new InMemoryStorage<EntityForEmployee>());
        }

        [Fact]
        public void Create_NullInstance_ThrowsException()
        {
            Action creation = () => _baseRepository.Create<EntityForEmployee>(null, null);
            creation.Should().Throw<ArgumentNullException>().Where(exc => exc.ParamName == "inMemoryInstance");
        }

        [Fact]
        public void Create_NullCloner_ThrowsException()
        {
            Action creation = () => _baseRepository.Create(new EntityForEmployee(1), null);
            creation.Should().Throw<ArgumentNullException>().Where(exc => exc.ParamName == "cloner");
        }

        [Fact]
        public void Create_EmptyStorage_IdIsOne()
        {
            var id = _baseRepository.Create(new EntityForEmployee(1), a => a);
            id.Should().Be(1);
        }

        [Fact]
        public void Get_EmptyStorage_ThrowsException()
        {
            Action getting = () => _baseRepository.Get(1);
            getting.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void Get_ExistingItem_ReturnsIt()
        {
            var initialItem = new EntityForEmployee(1029) { Date = DateTime.Now };
            var id = _baseRepository.Create(initialItem, a => a);

            var item = _baseRepository.Get(id);

            item.Date.Should().Be(initialItem.Date);
            item.EmployeeId.Should().Be(initialItem.EmployeeId);
            item.Id.Should().Be(id);
        }

        [Fact]
        public void GetForEmployee_EmptyStorage_ThrowsException()
        {
            Action getting = () => _baseRepository.GetForEmployee(1);
            getting.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void GetForEmployee_PresentEntity_ReturnsTheEntities()
        {
            var initialItem = new EntityForEmployee(1029) { Date = DateTime.Now };
            var id = _baseRepository.Create(initialItem, a => a);

            var items = _baseRepository.GetForEmployee(initialItem.EmployeeId);

            items.Should().OnlyContain(i => i.Id == id
                                           && i.EmployeeId == initialItem.EmployeeId
                                           && i.Date == initialItem.Date);
        }

        [Fact]
        public void GetForEmployee_PresentEntityTooEarly_ThrowsException()
        {
            var initialItem = new EntityForEmployee(1029) { Date = DateTime.Now };
            var id = _baseRepository.Create(initialItem, a => a);

            Action getting = () => _baseRepository.GetForEmployee(initialItem.EmployeeId, DateTime.Today.AddDays(1));

            getting.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void GetForEmployee_PresentEntityTooLate_ThrowsException()
        {
            var initialItem = new EntityForEmployee(1029) { Date = DateTime.Now };
            var id = _baseRepository.Create(initialItem, a => a);

            Action getting = () => _baseRepository.GetForEmployee(initialItem.EmployeeId, null, DateTime.Today.AddDays(-1));

            getting.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void GetForEmployee_PresentEntityInTimeRange_ReturnsTheItem()
        {
            var initialItem = new EntityForEmployee(1029) { Date = DateTime.Now };
            var id = _baseRepository.Create(initialItem, a => a);

            var items = _baseRepository.GetForEmployee(initialItem.EmployeeId, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

            items.Should().OnlyContain(i => i.Id == id
                                            && i.Date == initialItem.Date
                                            && i.EmployeeId == initialItem.EmployeeId);
        }
    }
}
