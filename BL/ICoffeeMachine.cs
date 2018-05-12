namespace BL
{
    public interface ICoffeeMachine
    {
        void PushBrewButton();
        event MachineLight LightIsOn;

        void PourWaterIntoBoilersPot(int cups); //how many cups of water are you going to drink

        IWarmerPot GetWarmerPot();
        void SetWarmerPot(IWarmerPot pot);

        void PutCoffeePowderIntoFilter(double grammes);     //amount of coffee to be prepared
        void AddFilter();
    }

    public delegate void MachineLight(object sender, MachineLightArgs args);

    public class MachineLightArgs
    {
        public bool IsLightning { get; set; }
        public int Color { get; set; }
    }
}
