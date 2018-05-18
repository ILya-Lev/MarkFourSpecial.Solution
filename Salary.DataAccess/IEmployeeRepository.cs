using Salary.Models;

namespace Salary.DataAccess
{
    public interface IEmployeeRepository
    {
        int Create(Employee inMemoryInstance);
        Employee Delete(int employeeId);

        Employee Get(int employeeId);

        Employee UpdateName(int employeeId, string name);
        Employee UpdateAddress(int employeeId, string address);
        Employee UpdateHourly(int employeeId, decimal hourlyRate);
        Employee UpdateSalaried(int employeeId, decimal salary);
        Employee UpdateCommissioned(int employeeId, decimal salary, decimal rate);
        Employee UpdateTradeUnionCharge(int employeeId, decimal? charge);
    }
}
