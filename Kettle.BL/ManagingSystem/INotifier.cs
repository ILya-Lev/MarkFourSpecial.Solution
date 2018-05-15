namespace Kettle.BL.ManagingSystem
{
    public interface INotifier
    {
        void Beep();
        void LightOn(Color color);
        void LightOff();
        void DisplayTemperature(int temperature);
    }
}