using BL.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    public class CoffeeMachine : ICoffeeMachine
    {
        private readonly IManagingSystem _managingSystem;
        private readonly IFilteringSystem _filteringSystem;
        private readonly IHeatingSystem _warmingSystem;
        private readonly IHeatingSystem _boilingSystem;

        public CoffeeMachine(IManagingSystem managingSystem, IFilteringSystem filteringSystem, IHeatingSystem warmingSystem, IHeatingSystem boilingSystem)
        {
            _managingSystem = managingSystem;
            _filteringSystem = filteringSystem;
            _warmingSystem = warmingSystem;
            _boilingSystem = boilingSystem;
        }

        public void PushBrewButton()
        {
            _managingSystem.PushBrewButton();

            if (!_filteringSystem.HasFilter())
                throw new NoFilterException("Cannot brew coffee as there is no filter in the machine");

            if (!_filteringSystem.GetFilter().HasCoffee())
                throw new NoCoffeeException("Cannot brew coffee as there is no coffee powder in the machine");

            if (_boilingSystem.GetPot().IsEmpty)
                throw new NoWaterException("Cannot brew coffee as there is no water in the machines boiler");

            _boilingSystem.GetHeater().TurnOn();
            WaitForCoffeeCooking();
        }

        private async void WaitForCoffeeCooking()
        {
            await new TaskFactory().StartNew(() =>
            {
                while (_warmingSystem.GetPot().IsEmpty)
                    Thread.Sleep(1000);
            });
        }

        public event MachineLight LightIsOn;
        public void PourWaterIntoBoilersPot(int cups)   //todo: do we really need this argument in current model?
        {
            _boilingSystem.PourWaterIn();
        }

        public IWarmerPot GetWarmerPot()
        {
            return _warmingSystem.GetPot();
        }

        public void SetWarmerPot(IWarmerPot pot)
        {
            _warmingSystem.SetPot(pot);
        }

        public void PutCoffeePowderIntoFilter(double grammes)
        {
            _filteringSystem.GetFilter().AddCoffee();
        }

        public void AddFilter()
        {
            _filteringSystem.AddFilter();
        }
    }

    public interface IHeatingSystem
    {
        void PourWaterIn();
        IWarmerPot GetPot();
        void SetPot(IWarmerPot pot);
        IHeater GetHeater();
    }

    public interface IHeater
    {
        void TurnOn();
        void TurnOff();
    }

    public interface IFilteringSystem
    {
        IFilter GetFilter();
        void ReplaceFilter(IFilter brandNew);
        void AddFilter();
        bool HasFilter();
    }

    public interface IFilter
    {
        void AddCoffee();
        bool HasCoffee();
    }

    public interface IManagingSystem
    {
        void PushBrewButton();
    }
}