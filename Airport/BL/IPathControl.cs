using Airport.Models;
using Airport.Models.Enum;

namespace Airport.BL
{
    public interface IPathControl
    {
        List<Airplane> PlanesOnPath { get; }

        void InitiazlizeStations();
        Task LandToTakeoff(int airplaneID);
        Task DoEmergency(StationType stationType, int stationNumber, int SOSSeconds);
    }
}