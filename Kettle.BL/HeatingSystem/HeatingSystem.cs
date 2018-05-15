using BL.Exceptions;
using System;
using System.Threading;

namespace Kettle.BL.HeatingSystem
{
    public class HeatingSystem : IHeatingSystem
    {
        private readonly ISensor _sensor;
        private readonly IHeater _heater;

        public HeatingSystem(ISensor sensor, IHeater heater)
        {
            _sensor = sensor;
            _heater = heater;
        }

        public void HeatUpTo(int threshold)
        {
            if (!_sensor.IsWaterPresent)
                throw new NoWaterException("There is no water in the Kettle");

            while (_sensor.Temperature < threshold)
            {
                _heater.Start();
                Console.WriteLine($"Heating water; current temperature {_sensor.Temperature}");
                Thread.Sleep(100);
                _sensor.Temperature++;
            }
            _heater.Stop();
        }

        public void Boil() => HeatUpTo(Constants.BoilingTemperature);

        public void AddWater()
        {
            _sensor.IsWaterPresent = true;
            _sensor.Temperature = Constants.DefaultTemperature;
        }

        public void RemoveWater()
        {
            _sensor.IsWaterPresent = false;
            _sensor.Temperature = Constants.DefaultTemperature;
        }
    }
}