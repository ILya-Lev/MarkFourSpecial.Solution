namespace Kettle.BL.ManagingSystem
{
    public interface IMenu
    {
        void PressManageButton();
        void PressBoilButton();
        void PressLowerLimitButton();
        void PressSelectionButton();

        bool ManageBtnPressed { get; }
        int TargetTemperature { get; }
    }
}