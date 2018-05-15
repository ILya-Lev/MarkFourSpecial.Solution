using Salary.Models;

namespace Salary.DataAccess
{
    public interface IEmployeeRepository
    {
        int Create(Employee inMemoryInstance);
        Employee Get(int employeeId);
        Employee Update(Employee editedEmployee);
        Employee Delete(int employeeId);
    }
}
