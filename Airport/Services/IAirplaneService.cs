using Airport.BL.StationControl;
using Airport.Models;

namespace Airport.Services
{
    public interface IAirplaneService
    {
        public void SortAirplanes(List<AirplaneModel> listOnPath, Action<AirplaneModel> action);
        public void SendStation(Station ap);
        public List<AirplaneModel> Airplanes { get; set; } 
        public List<StationModel> Stations { get; set; } 
    }
}