using Kettle.BL.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kettle.BL.ManagingSystem
{
    public class Menu : IMenu, INotifyPropertyChanged
    {
        private bool _manageBtnPressed = false;
        private int _targetTemperature = 0;

        public bool ManageBtnPressed
        {
            get => _manageBtnPressed;
            private set
            {
                _manageBtnPressed = value;
                OnPropertyChanged();
            }
        }

        public int TargetTemperature
        {
            get => _targetTemperature;
            private set
            {
                _targetTemperature = value;
                OnPropertyChanged();
            }
        }

        public void PressManageButton() => ManageBtnPressed = !ManageBtnPressed;

        public void PressBoilButton() => TargetTemperature = Constants.BoilingTemperature;

        public void PressLowerLimitButton() => TargetTemperature = Constants.LowerTemperature;

        public void PressSelectionButton() => TargetTemperature += 5;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}