using System;
using System.Collections.Generic;
using BobsMachine.Abstractions;
using BobsMachine.Api;

namespace BobsMachine.Implementation
{
    public class M4ContainmentVessel : ContainmentVessel, IPollable
    {
        private readonly ICoffeeMakerApi _api;
        private WarmerPlateStatus _lastPotStatus = WarmerPlateStatus.PotEmpty;

        public M4ContainmentVessel(ICoffeeMakerApi api)
        {
            _api = api;
        }

        public override bool IsReady()
        {
            var plateStatus = _api.GetWarmerPlateStatus();
            return plateStatus == WarmerPlateStatus.PotEmpty;
        }

        public void Poll()
        {
            var potStatus = _api.GetWarmerPlateStatus();
            if (potStatus == _lastPotStatus)
                return;

            if (_isBrewing)
            {
                HandleBrevingEvent(potStatus);
            }
            else if (!_isComplete)
            {
                HandleIncompleteEvent(potStatus);
            }

            _lastPotStatus = potStatus;
        }

        private void HandleBrevingEvent(WarmerPlateStatus potStatus)
        {
            var handlers = new Dictionary<WarmerPlateStatus, Action>()
            {
                [WarmerPlateStatus.PotNotEmpty] = () =>
                {
                    ContainerAvailable();
                    _api.SetWarmerState(WarmerState.On);
                },
                [WarmerPlateStatus.WarmerEmpty] = () =>
                {
                    ContainerUnavailable();
                    _api.SetWarmerState(WarmerState.Off);
                },
                [WarmerPlateStatus.PotEmpty] = () =>
                {
                    ContainerAvailable();
                    _api.SetWarmerState(WarmerState.Off);
                }
            };

            handlers[potStatus]();
        }

        private void HandleIncompleteEvent(WarmerPlateStatus potStatus)
        {
            var handlers = new Dictionary<WarmerPlateStatus, Action>()
            {
                [WarmerPlateStatus.PotNotEmpty] = () => _api.SetWarmerState(WarmerState.On),
                [WarmerPlateStatus.WarmerEmpty] = () => _api.SetWarmerState(WarmerState.Off),
                [WarmerPlateStatus.PotEmpty] = () =>
                {
                    _api.SetWarmerState(WarmerState.Off);
                    DeclareComplete();

                }
            };

            handlers[potStatus]();
        }
    }
}