using BobsMachine.Abstractions;
using BobsMachine.Api;

namespace BobsMachine.Implementation
{
    public class M4HotWaterSource : HotWaterSource, IPollable
    {
        private readonly ICoffeeMakerApi _api;

        public M4HotWaterSource(ICoffeeMakerApi api)
        {
            _api = api;
        }

        public override bool IsReady()
        {
            var boilerStatus = _api.GetBoilerStatus();
            return boilerStatus == BoilerStatus.NotEmpty;
        }

        public override void StartBrewing()
        {
            _api.SetReliefValveState(ReliefValveState.Closed);
            _api.SetBoilerState(BoilerState.On);
        }

        public override void Pause()
        {
            _api.SetBoilerState(BoilerState.Off);
            _api.SetReliefValveState(ReliefValveState.Open);
        }

        public override void Resume()
        {
            _api.SetBoilerState(BoilerState.On);
            _api.SetReliefValveState(ReliefValveState.Closed);
        }

        public void Poll()
        {
            var boilerStatus = _api.GetBoilerStatus();
            if (_isBrewing && boilerStatus == BoilerStatus.Empty)
            {
                _api.SetBoilerState(BoilerState.Off);
                _api.SetReliefValveState(ReliefValveState.Closed);
                DeclareDone();
            }
        }
    }
}
