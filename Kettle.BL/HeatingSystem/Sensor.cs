namespace Kettle.BL.HeatingSystem
{
    public class Sensor : ISensor
    {
        public int Temperature { get; set; } = Constants.DefaultTemperature;
        public bool IsWaterPresent { get; set; } = false;
    }
}