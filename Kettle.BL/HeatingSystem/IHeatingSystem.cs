namespace Kettle.BL.HeatingSystem
{
    public interface IHeatingSystem
    {
        void HeatUpTo(int threshold);
        void Boil();
        void AddWater();
        void RemoveWater(); //the system itself cannot either add or remove water, but I need somehow change sensor state
    }
}