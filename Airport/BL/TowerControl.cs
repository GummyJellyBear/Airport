using Airport.Models;
using Airport.Models.Enum;
using Airport.Services;

namespace Airport.BL
{
    public class TowerControl : ITowerControl
    {
        private readonly IAirplaneService _airplaneService;
        private readonly IPathControl _pathControl;
        private readonly ITimerService _backgroundTimer;
        private readonly IAirControl _airControl;
        public TowerControl(
            IAirplaneService airplaneService,
            IPathControl pathControl,
            ITimerService background,
            IAirControl airControl)
        {
            _airplaneService = airplaneService;
            _pathControl = pathControl;
            _backgroundTimer = background;
            _airControl = airControl;
        }
        public void InitiazlizeStations() => _pathControl.InitiazlizeStations();
        public void InitiazAirMovment() => _airControl.FlyTimer(_pathControl.PlanesOnPath);
        public void InitiazlizePlanes() => _airControl.PlanesToLand = _airplaneService.GetPlanes().ToList();
        public void StartSimulator()
        {
            var listOnPath = _pathControl.PlanesOnPath;
            _backgroundTimer.Start(() =>
            {
                if (listOnPath.Count != 0)
                {
                    var plane = listOnPath[0];
                    listOnPath.RemoveAt(0);
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        _pathControl.LandToTakeoff(plane.Id);
                    }));
                    thread.Start();
                }
            });
        }
        public void DoEmergency(StationType stationType, int stationNumber, int SOSSeconds) => 
            _pathControl.DoEmergency(stationType, stationNumber, SOSSeconds);
    }
}


