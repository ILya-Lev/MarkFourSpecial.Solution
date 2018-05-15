namespace BobsMachine.Api
{
    public class CoffeeMakerApi : ICoffeeMakerApi
    {
        private WarmerPlateStatus _warmerStatus = WarmerPlateStatus.PotEmpty;
        private BoilerStatus _boilerStatus = BoilerStatus.NotEmpty;
        private BrewButtonStatus _brewButtonStatus = BrewButtonStatus.NotPushed;

        public WarmerPlateStatus GetWarmerPlateStatus() => _warmerStatus;

        public BoilerStatus GetBoilerStatus() => _boilerStatus;

        public BrewButtonStatus GetBrewButtonStatus() => _brewButtonStatus;

        public void SetBoilerState(BoilerState state)
        {
            if (state == BoilerState.On)
            {
                _boilerStatus = BoilerStatus.NotEmpty;
                _warmerStatus = WarmerPlateStatus.PotEmpty;
                _brewButtonStatus = BrewButtonStatus.Pushed;
                return;
            }
            _boilerStatus = BoilerStatus.Empty;
            _warmerStatus = WarmerPlateStatus.PotNotEmpty;
            _brewButtonStatus = BrewButtonStatus.NotPushed;
        }

        public void SetWarmerState(WarmerState state)
        {
            if (state == WarmerState.On)
            {
                _boilerStatus = BoilerStatus.NotEmpty;
                _warmerStatus = WarmerPlateStatus.PotNotEmpty;
                _brewButtonStatus = BrewButtonStatus.Pushed;
                return;
            }
            _boilerStatus = BoilerStatus.Empty;
            _warmerStatus = WarmerPlateStatus.WarmerEmpty;
            _brewButtonStatus = BrewButtonStatus.NotPushed;
        }

        public void SetIndicatorState(IndicatorState state)
        {
            if (state == IndicatorState.On)
            {
                _boilerStatus = BoilerStatus.Empty;
                _warmerStatus = WarmerPlateStatus.PotNotEmpty;
                _brewButtonStatus = BrewButtonStatus.NotPushed;
                return;
            }
            _boilerStatus = BoilerStatus.NotEmpty;
            _warmerStatus = WarmerPlateStatus.PotEmpty;
            _brewButtonStatus = BrewButtonStatus.Pushed;
        }

        public void SetReliefValveState(ReliefValveState state)
        {
            if (state == ReliefValveState.Open)
            {
                _boilerStatus = BoilerStatus.NotEmpty;
                _warmerStatus = WarmerPlateStatus.PotEmpty;
                _brewButtonStatus = BrewButtonStatus.Pushed;
                return;
            }
            _boilerStatus = BoilerStatus.Empty;
            _warmerStatus = WarmerPlateStatus.PotNotEmpty;
            _brewButtonStatus = BrewButtonStatus.NotPushed;
        }
    }
}