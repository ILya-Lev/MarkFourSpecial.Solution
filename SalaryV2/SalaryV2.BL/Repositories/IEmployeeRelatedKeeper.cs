using SalaryV2.BL.Models;
using SalaryV2.BL.Transactions;

namespace SalaryV2.BL.Repositories
{
    public interface IEmployeeRelatedKeeper<in T> where T : EntityForEmployee
    {
        int Add(T entity);
    }
}