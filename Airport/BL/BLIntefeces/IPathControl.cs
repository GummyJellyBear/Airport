using Airport.Hubs;
using Airport.Models;
using Airport.Models.Enum;
using Microsoft.AspNetCore.SignalR;

namespace Airport.BL.BLIntefeces
{
    public interface IPathControl
    {
        List<AirplaneModel> PlanesOnPath { get; }
        void InitiazlizeStations(IHubContext<MyHub> context);
        Task LandToTakeoff(AirplaneModel ap);
        public Task DoEmergency(int stationNumber, int SOSSeconds);
        public void DoRain(int time);
        public Task Land(AirplaneModel ap);
        public Task Takeoff(AirplaneModel ap);
    }
}