namespace Salary.Services
{
    public interface IPayrollStrategyFactory
    {
        IPayrollStrategy GetStrategy(int employeeId);
    }
}