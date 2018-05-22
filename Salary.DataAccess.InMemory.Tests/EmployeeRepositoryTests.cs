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
    public class EmployeeRepositoryTests
    {
        private readonly InMemoryEmployeeRepository _repository;
        public EmployeeRepositoryTests()
        {
            _repository = new InMemoryEmployeeRepository();
        }

        [Fact]
        public void Create_NotZeroId_ThrowsException()
        {
            var instance = new Employee() { MajorRate = 111m };
            instance.Id = _repository.Create(instance);

            Action creation = () => _repository.Create(instance);

            creation.Should()
                .Throw<RepositoryException>("in memory instance already has an id => is considered as stored")
                .Where(exc => exc.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public void Create_NullInstance_ThrowsException()
        {
            Action creation = () => _repository.Create(null);
            creation.Should().Throw<ArgumentNullException>().Where(exc => exc.ParamName == "employee");
        }

        [Fact]
        public void Create_EmptyStorage_IdIsOne()
        {
            var instance = new Employee() { MajorRate = 111m };
            var id = _repository.Create(instance);
            id.Should().Be(1, "id starts from 1, storage is empty => new item has id = 1");
        }

        [Fact]
        public void Create_StorageContainsItems_IdEqualsAmountOfItemsInStorage()
        {
            var instance = new Employee() { MajorRate = 111m };
            var ids = new List<int>();

            var employeesAmount = 10;
            for (int i = 0; i < employeesAmount; i++)
            {
                ids.Add(_repository.Create(instance));
            }

            ids.Should().BeEquivalentTo(Enumerable.Range(1, employeesAmount));
        }

        [InlineData(PaymentType.Hourly)]
        [InlineData(PaymentType.Monthly)]
        [Theory]
        public void Create_NonEmptyMinorRateContradictsPaymentType_ThrowsException(PaymentType paymentType)
        {
            var instance = new Employee() { PaymentType = paymentType, MajorRate = 3200m, MinorRate = 2m };
            Action creation = () => _repository.Create(instance);
            creation.Should().Throw<ValidationException>();
        }

        [InlineData(PaymentType.Hourly)]
        [InlineData(PaymentType.Monthly)]
        [Theory]
        public void Create_EmptyMajorRateForAnyPaymentType_ThrowsException(PaymentType paymentType)
        {
            var instance = new Employee() { PaymentType = paymentType };
            Action creation = () => _repository.Create(instance);
            creation.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Create_EmptyMinorRateForCommissioned_ThrowsException()
        {
            var instance = new Employee() { PaymentType = PaymentType.Commissioned, MajorRate = 3200m };
            Action creation = () => _repository.Create(instance);
            creation.Should().Throw<ValidationException>();
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
            _repository.Create(new Employee() { MajorRate = 111m });
            Action deletion = () => _repository.Delete(2);
            deletion.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound); ;
        }

        [Fact]
        public void Delete_StorageContainsSpecifiedId_ReturnsTheEmployee()
        {
            var id = _repository.Create(new Employee { Name = "Anatole", MajorRate = 10m });
            var anatole = _repository.Delete(id);
            anatole.Name.Should().Be("Anatole");
        }

        [Fact]
        public void Create_AfterDeletion_IdIsReused()
        {
            var id = _repository.Create(new Employee { Name = "Anatole", MajorRate = 10m });
            var anatole = _repository.Delete(id);
            anatole.Name.Should().Be("Anatole");

            var anotherId = _repository.Create(new Employee { Name = "Inakij", MajorRate = 11m });
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
            var id = _repository.Create(new Employee { Name = "Adam", MajorRate = 10m });
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
                _repository.Create(new Employee { Name = i.ToString(), MajorRate = 10m });
            }
            var employees = _repository.GetAll();
            employees.Should().HaveCount(3);
            employees.Should().OnlyContain(empl => new[] { "0", "1", "2" }.Contains(empl.Name));
        }

        [Fact]
        public void UpdateName_AbsentId_ThrowsException()
        {
            Action nameUpdate = () => _repository.UpdateName(1, "vasya");
            nameUpdate.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void UpdateName_PresentId_ChangesName()
        {
            var id = _repository.Create(new Employee { Name = "John", MajorRate = 10m });
            var updatedEmployee = _repository.UpdateName(id, "Jack");
            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.Name.Should().Be("Jack");
        }

        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [Theory]
        public void UpdateName_PresentIdEmptyName_ThrowsException(string newName)
        {
            var id = _repository.Create(new Employee { Name = "John", MajorRate = 10m });
            Action setEmptyName = () => _repository.UpdateName(id, newName);

            setEmptyName.Should().Throw<ValidationException>().Where(exc => exc.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public void UpdateAddress_AbsentId_ThrowsException()
        {
            Action nameUpdate = () => _repository.UpdateAddress(1, "vasya");
            nameUpdate.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("Kharkov")]
        [Theory]
        public void UpdateAddress_PresentId_ChangesAddress(string newAddress)
        {
            var id = _repository.Create(new Employee { Address = "Kiev", MajorRate = 10m });
            var updatedEmployee = _repository.UpdateAddress(id, newAddress);
            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.Address.Should().Be(newAddress);
        }

        [Fact]
        public void UpdateHourly_AbsentId_ThrowsException()
        {
            Action nameUpdate = () => _repository.UpdateHourly(1, 10m);
            nameUpdate.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void UpdateHourly_AlreadyHourly_ChangesRate()
        {
            var id = _repository.Create(new Employee { PaymentType = PaymentType.Hourly, MajorRate = 10m });

            var updatedEmployee = _repository.UpdateHourly(id, 12m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(12m);
            updatedEmployee.PaymentType.Should().Be(PaymentType.Hourly);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateHourly_FromMonthly_ChangesRateAndType()
        {
            var id = _repository.Create(new Employee { PaymentType = PaymentType.Monthly, MajorRate = 3200m });

            var updatedEmployee = _repository.UpdateHourly(id, 12m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(12m);
            updatedEmployee.PaymentType.Should().Be(PaymentType.Hourly);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateHourly_FromCommissioned_ChangesRatesAndType()
        {
            var id = _repository.Create(new Employee
            {
                PaymentType = PaymentType.Commissioned,
                MajorRate = 3200m,
                MinorRate = 2m
            });

            var updatedEmployee = _repository.UpdateHourly(id, 12m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(12m);
            updatedEmployee.MinorRate.Should().BeNull();
            updatedEmployee.PaymentType.Should().Be(PaymentType.Hourly);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateMonthly_AbsentId_ThrowsException()
        {
            Action nameUpdate = () => _repository.UpdateMonthly(1, 10m);
            nameUpdate.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void UpdateMonthly_AlreadyMonthly_ChangesRate()
        {
            var id = _repository.Create(new Employee { PaymentType = PaymentType.Monthly, MajorRate = 2300m });

            var updatedEmployee = _repository.UpdateMonthly(id, 3200m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(3200m);
            updatedEmployee.PaymentType.Should().Be(PaymentType.Monthly);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateMonthly_FromHourly_ChangesRateAndType()
        {
            var id = _repository.Create(new Employee { PaymentType = PaymentType.Hourly, MajorRate = 10m });

            var updatedEmployee = _repository.UpdateMonthly(id, 3200m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(3200m);
            updatedEmployee.PaymentType.Should().Be(PaymentType.Monthly);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateMonthly_FromCommissioned_ChangesRatesAndType()
        {
            var id = _repository.Create(new Employee
            {
                PaymentType = PaymentType.Commissioned,
                MajorRate = 2300m,
                MinorRate = 2m
            });

            var updatedEmployee = _repository.UpdateMonthly(id, 3200m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(3200m);
            updatedEmployee.MinorRate.Should().BeNull();
            updatedEmployee.PaymentType.Should().Be(PaymentType.Monthly);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateCommissioned_AbsentId_ThrowsException()
        {
            Action nameUpdate = () => _repository.UpdateCommissioned(1, 10m, 3m);
            nameUpdate.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void UpdateCommissioned_AlreadyCommissioned_ChangesRates()
        {
            var id = _repository.Create(new Employee
            {
                PaymentType = PaymentType.Commissioned,
                MajorRate = 2300m,
                MinorRate = 3m
            });

            var updatedEmployee = _repository.UpdateCommissioned(id, 3200m, 4m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(3200m);
            updatedEmployee.MinorRate.Should().Be(4m);
            updatedEmployee.PaymentType.Should().Be(PaymentType.Commissioned);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateCommissioned_FromHourly_ChangesRateAndType()
        {
            var id = _repository.Create(new Employee { PaymentType = PaymentType.Hourly, MajorRate = 10m });

            var updatedEmployee = _repository.UpdateCommissioned(id, 3200m, 5m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(3200m);
            updatedEmployee.MinorRate.Should().Be(5m);
            updatedEmployee.PaymentType.Should().Be(PaymentType.Commissioned);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateCommissioned_FromMonthly_ChangesRatesAndType()
        {
            var id = _repository.Create(new Employee { PaymentType = PaymentType.Monthly, MajorRate = 2300m });

            var updatedEmployee = _repository.UpdateCommissioned(id, 3200m, 6m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.MajorRate.Should().Be(3200m);
            updatedEmployee.MinorRate.Should().Be(6m);
            updatedEmployee.PaymentType.Should().Be(PaymentType.Commissioned);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateTradeUnionCharge_AbsentId_ThrowsException()
        {
            Action nameUpdate = () => _repository.UpdateTradeUnionCharge(1, 100m);
            nameUpdate.Should().Throw<RepositoryException>().Where(exc => exc.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void UpdateTradeUnionCharge_AlreadyInTradeUnion_SetsUpCharge()
        {
            var id = _repository.Create(new Employee
            {
                MajorRate = 2300m,
                TradeUnionCharge = 10m
            });

            var updatedEmployee = _repository.UpdateTradeUnionCharge(id, 20m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.TradeUnionCharge.Should().Be(20m);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }

        [Fact]
        public void UpdateTradeUnionCharge_NotAMemberYet_SetsUpCharge()
        {
            var id = _repository.Create(new Employee { PaymentType = PaymentType.Hourly, MajorRate = 10m });
            _repository.Get(id).TradeUnionCharge.Should().BeNull();

            var updatedEmployee = _repository.UpdateTradeUnionCharge(id, 100m);

            updatedEmployee.Id.Should().Be(id);
            updatedEmployee.TradeUnionCharge.Should().Be(100m);


            var empl = _repository.Get(id);
            empl.Should().Be(updatedEmployee);
        }
    }
}
