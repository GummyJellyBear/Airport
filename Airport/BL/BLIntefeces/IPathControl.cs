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
        Task DoEmergency(int stationNumber, int SOSSeconds);
    }
}