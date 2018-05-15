using Kettle.BL.HeatingSystem;
using Kettle.BL.ManagingSystem;

namespace Kettle.BL
{
    public class Kettle
    {
        private readonly IHeatingSystem _heatingSystem;
        private readonly INotifier _notifier;
        private readonly IMenu _menu;

        public Kettle(IHeatingSystem heatingSystem, INotifier notifier, IMenu menu)
        {
            _heatingSystem = heatingSystem;
            _notifier = notifier;
            _menu = menu;
        }

        /// <summary>
        /// emulates user interaction with the kettle
        /// </summary>
        public void HeatWater()
        {
            _menu.PressManageButton();

            _notifier.Beep();
            //todo:place a strategy here!
            _notifier.LightOn(Color.White);
            _notifier.DisplayTemperature(_menu.TargetTemperature);

            _heatingSystem.HeatUpTo(_menu.TargetTemperature);

        }
    }

    public enum Color
    {
        White,
        Blue,
        Green,
        Yellow,
        Red
    };
}
