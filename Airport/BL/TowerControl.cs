using Airport.BL.BLIntefeces;
using Airport.Hubs;
using Airport.Models;
using Airport.Models.Enum;
using Airport.Services;
using Microsoft.AspNetCore.SignalR;

namespace Airport.BL
{
    public class TowerControl : ITowerControl
    {
        private readonly IAirplaneService _airplaneService;
        private readonly IPathControl _pathControl;
        private readonly ITimerService _backgroundTimer;
        private readonly ITimerService _backgroundRender;
        private readonly IAirControl _airControl;
        private readonly IGroundControl _groundControl;
        private readonly IHubContext<MyHub> _hub;

        public TowerControl(
            IAirplaneService airplaneService,
            IPathControl pathControl,
            ITimerService background,
            ITimerService backgroundRender,
            IAirControl airControl,
            IGroundControl groungControl,
            IHubContext<MyHub> hub)
        {
            _airplaneService = airplaneService;
            _pathControl = pathControl;
            _backgroundTimer = background;
            _backgroundRender = backgroundRender;
            _airControl = airControl;
            _groundControl = groungControl;
            _hub = hub;
        }
        public void StartSimulator()
        {
            InitiazlizeStations();
            InitiazAirMovment();
            InitiazlizePlanes();
            var listOnPathFromAir = _airplaneService.PlanesOnPath;

            _airplaneService.QueueAirplanes(listOnPathFromAir, (plane) =>
                _pathControl.LandToTakeoff(plane));
        }
        public void RenderAirplanes()
        {
            _backgroundRender.Start(async () =>
            {
                foreach (var station in _airplaneService.Stations)
                {
                    var s = new StationModel() { AirplaneInIt = station.AirplaneInIt, StationID = station.StationID };
                    //await _hub.Clients.All.SendAsync("onAirplaneJoin",
                    //    station.AirplaneInIt, station.StationID);
                    await _hub.Clients.All.SendAsync("onAirplaneJoin", s);
                }
            });
        }
        public void DoEmergency(int stationNumber, int SOSSeconds) =>
            _pathControl.DoEmergency(stationNumber, SOSSeconds);
        void InitiazlizeStations() => _pathControl.InitiazlizeStations(_hub);
        void InitiazAirMovment() => _airControl.FlyTimer(_airplaneService.PlanesOnPath);
        void InitiazlizePlanes() => _airControl.PlanesToLand = _airplaneService.Airplanes;
    }
}


