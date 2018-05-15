namespace Kettle.BL.HeatingSystem
{
    public interface ISensor
    {
        int Temperature { get; set; }
        bool IsWaterPresent { get; set; }
    }
}