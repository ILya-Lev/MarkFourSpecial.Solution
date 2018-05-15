namespace BobsMachine.Abstractions
{
    public abstract class HotWaterSource
    {
        private UserInterface _userInterface;
        private ContainmentVessel _containmentVessel;

        protected bool _isBrewing = false;

        public void Initialize(UserInterface userInterface, ContainmentVessel containmentVessel)
        {
            _userInterface = userInterface;
            _containmentVessel = containmentVessel;
        }

        public void Start()
        {
            _isBrewing = true;
            StartBrewing();
        }

        public void Done() => _isBrewing = false;

        protected void DeclareDone()
        {
            _userInterface.Done();
            _containmentVessel.Done();
            Done();
        }

        public abstract bool IsReady();
        public abstract void StartBrewing();
        public abstract void Pause();
        public abstract void Resume();
    }
}
