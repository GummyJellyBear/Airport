using Airport.BL.StationControl;
using Airport.Models;

namespace Airport.Services
{
    public interface IAirplaneService
    {
        public Task QueueAirplanes(List<AirplaneModel> listOnPath, Action<AirplaneModel> action);
        public void SendStation(Station ap);
        public List<AirplaneModel> PlanesOnPath { get; }
        public List<AirplaneModel> PlanesParking { get; }
        public List<AirplaneModel> Airplanes { get; set; } 
        public List<StationModel> Stations { get; set; } 
    }
}