using System;
using System.Threading;

namespace Kettle.BL.ManagingSystem
{
    public class Notifier : INotifier
    {
        public void Beep() => Console.WriteLine("Beep!");

        public void LightOn(Color color) => Console.WriteLine($"Light is on with color {color}");

        public void LightOff() => Console.WriteLine("Light is off");

        public void DisplayTemperature(int temperature)
        {
            Console.WriteLine($"Temperature is {temperature}");
            Thread.Sleep(5000);
        }
    }
}