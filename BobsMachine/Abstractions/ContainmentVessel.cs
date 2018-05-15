namespace BobsMachine.Abstractions
{
    public abstract class ContainmentVessel
    {
        private UserInterface _userInterface;
        private HotWaterSource _hotWaterSource;

        protected bool _isBrewing = false;
        protected bool _isComplete = true;

        public void Init(UserInterface userInterface, HotWaterSource hotWaterSource)
        {
            _userInterface = userInterface;
            _hotWaterSource = hotWaterSource;
        }

        public void Start()
        {
            _isBrewing = true;
            _isComplete = false;
        }

        public void Done() => _isBrewing = false;

        protected void DeclareComplete()
        {
            _isComplete = true;
            _userInterface.Complete();
        }

        protected void ContainerAvailable() => _hotWaterSource.Resume();

        protected void ContainerUnavailable() => _hotWaterSource.Pause();

        public abstract bool IsReady();
    }
}