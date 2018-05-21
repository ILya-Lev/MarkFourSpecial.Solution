using FluentAssertions;
using Salary.Models;
using Salary.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace Salary.DataAccess.InMemory.Tests
{
    public class InMemoryEmployeeRepositoryTests
    {
        private readonly InMemoryEmployeeRepository _repository;
        public InMemoryEmployeeRepositoryTests()
        {
            _repository = new InMemoryEmployeeRepository();
        }

        [Fact]
        public void Create_NotZeroId_ThrowsException()
        {
            var instance = new Employee();
            instance.Id = _repository.Create(instance);

            Action creation = () => _repository.Create(instance);

            creation.Should()
                .Throw<RepositoryException>("in memory instance already has an id => is considered as stored")
                .Where(exc => exc.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public void Create_EmptyStorage_IdIsOne()
        {
            var instance = new Employee();
            var id = _repository.Create(instance);
            id.Should().Be(1, "id starts from 1, storage is empty => new item has id = 1");
        }

        [Fact]
        public void Create_StorageContainsItems_IdEqualsAmountOfItemsInStorage()
        {
            var instance = new Employee();
            var ids = new List<int>();

            var employeesAmount = 10;
            for (int i = 0; i < employeesAmount; i++)
            {
                ids.Add(_repository.Create(instance));
            }

            ids.Should().BeEquivalentTo(Enumerable.Range(1, employeesAmount));
        }

        [Fact]
        public void Delete_StorageIsEmpty_ThrowsException()
        {
            Action deletion = () => _repository.Delete(1);
            deletion.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void Delete_StorageNotEmptyButSpecifiedIdAbsent_ThrowsException()
        {
            _repository.Create(new Employee());
            Action deletion = () => _repository.Delete(2);
            deletion.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound); ;
        }

        [Fact]
        public void Delete_StorageContainsSpecifiedId_ReturnsTheEmployee()
        {
            var id = _repository.Create(new Employee { Name = "Anatole" });
            var anatole = _repository.Delete(id);
            anatole.Name.Should().Be("Anatole");
        }

        [Fact]
        public void Create_AfterDeletion_IdIsReused()
        {
            var id = _repository.Create(new Employee { Name = "Anatole" });
            var anatole = _repository.Delete(id);
            anatole.Name.Should().Be("Anatole");

            var anotherId = _repository.Create(new Employee { Name = "Inakij" });
            anotherId.Should().Be(id);
        }

        [Fact]
        public void Get_IdIsAbsent_ThrowsException()
        {
            Action getting = () => _repository.Get(1);
            getting.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void Get_IdIsPresents_ReturnsEmployee()
        {
            var id = _repository.Create(new Employee { Name = "Adam" });
            var employee = _repository.Get(id);
            employee.Name.Should().Be("Adam");
        }

        [Fact]
        public void GetAll_EmptyStorage_EmptyCollection()
        {
            var employees = _repository.GetAll();
            employees.Should().BeEmpty();
        }

        [Fact]
        public void GetAll_Contains3Items_All3Items()
        {
            for (int i = 0; i < 3; i++)
            {
                _repository.Create(new Employee { Name = i.ToString() });
            }
            var employees = _repository.GetAll();
            employees.Should().HaveCount(3);
            employees.Should().OnlyContain(empl => new[] { "0", "1", "2" }.Contains(empl.Name));
        }
    }
}
