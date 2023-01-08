using Microsoft.AspNetCore.SignalR;

namespace Airport.Models
{
    public class StationModel
    {
        public int StationID { get; set; }
        public AirplaneModel? AirplaneInIt { get; set; }
    }
}
