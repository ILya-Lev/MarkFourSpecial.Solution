using Salary.DataAccess;

namespace Salary.Services.Implementation.Factories
{
    public class ChargeStrategyFactory : IChargeStrategyFactory
    {
        private readonly IChargeStrategy _tradeUnion;
        private readonly IChargeStrategy _none;
        private readonly IEmployeeRepository _employeeRepository;

        public ChargeStrategyFactory(IChargeStrategy tradeUnion, IChargeStrategy none, IEmployeeRepository employeeRepository)
        {
            _tradeUnion = tradeUnion;
            _none = none;
            _employeeRepository = employeeRepository;
        }

        public IChargeStrategy GetStrategy(int employeeId)
        {
            var employee = _employeeRepository.Get(employeeId);
            return employee.TradeUnionCharge.HasValue ? _tradeUnion : _none;
        }
    }
}
