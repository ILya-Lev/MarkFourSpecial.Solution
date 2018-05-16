namespace Salary.Services
{
    public interface IChargeStrategyFactory
    {
        IChargeStrategy GetStrategy(int employeeId);
    }
}