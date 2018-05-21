using System;

namespace Salary.Services
{
    public interface IChargeStrategy
    {
        decimal GetCharge(int employeeId, DateTime forDate);
    }

    public interface ITradeUnionChargeStrategy : IChargeStrategy { }
    public interface INoneChargeStrategy : IChargeStrategy { }
}