using System;

namespace Kettle.BL.HeatingSystem
{
    public class Heater : IHeater
    {
        public void Start() => Console.WriteLine("Started heating");

        public void Stop() => Console.WriteLine("Stopped heating");
    }
}