using SalaryV2.BL.Models;

namespace SalaryV2.BL.Repositories
{
    public interface IEmployeeRepository
    {
        int Create(Employee employee);
    }
}