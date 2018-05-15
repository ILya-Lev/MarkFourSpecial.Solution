namespace BobsMachine.Api
{
    public enum WarmerPlateStatus { WarmerEmpty, PotEmpty, PotNotEmpty }
    public enum BoilerStatus { Empty, NotEmpty }
    public enum BrewButtonStatus { Pushed, NotPushed }
    public enum BoilerState { On, Off }
    public enum WarmerState { On, Off }
    public enum IndicatorState { On, Off }
    public enum ReliefValveState { Open, Closed }

    public interface ICoffeeMakerApi
    {
        WarmerPlateStatus GetWarmerPlateStatus();
        BoilerStatus GetBoilerStatus();
        BrewButtonStatus GetBrewButtonStatus();

        void SetBoilerState(BoilerState state);
        void SetWarmerState(WarmerState state);
        void SetIndicatorState(IndicatorState state);
        void SetReliefValveState(ReliefValveState state);
    }
}