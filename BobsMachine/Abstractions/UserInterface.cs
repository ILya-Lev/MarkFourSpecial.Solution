namespace BobsMachine.Abstractions
{
    public abstract class UserInterface
    {
        protected bool _isComplete = true;

        private HotWaterSource _hotWaterSource;
        private ContainmentVessel _containmentVessel;

        public void Initialize(HotWaterSource hotWaterSource, ContainmentVessel containmentVessel)
        {
            _hotWaterSource = hotWaterSource;
            _containmentVessel = containmentVessel;
        }

        public void Complete()
        {
            _isComplete = true;
            CompleteCycle();
        }

        protected void StartBrewing()
        {
            if (_hotWaterSource.IsReady() && _containmentVessel.IsReady())
            {
                _isComplete = false;
                _hotWaterSource.Start();
                _containmentVessel.Start();
            }
        }

        public abstract void Done();
        public abstract void CompleteCycle();
    }
}