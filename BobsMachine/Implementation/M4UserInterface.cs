using BobsMachine.Abstractions;
using BobsMachine.Api;

namespace BobsMachine.Implementation
{
    public class M4UserInterface : UserInterface, IPollable
    {
        private readonly ICoffeeMakerApi _api;

        public M4UserInterface(ICoffeeMakerApi api)
        {
            _api = api;
        }

        public void Poll()
        {
            var buttonStatus = _api.GetBrewButtonStatus();
            if (buttonStatus == BrewButtonStatus.Pushed)
            {
                StartBrewing();
            }
        }

        public override void Done()
        {
            _api.SetIndicatorState(IndicatorState.On);
        }

        public override void CompleteCycle()
        {
            _api.SetIndicatorState(IndicatorState.Off);
        }
    }
}
