using Airport.Hubs;
using Airport.Models;
using Airport.Services;
using Microsoft.AspNetCore.SignalR;

namespace Airport.BL.StationControl
{
    public class StationPath
    {
        private readonly IAirplaneService _airplaneService;

        public StationPath(IAirplaneService airplaneService) => _airplaneService = airplaneService;
        public Station[] Stations { get; set; }
        public StationPath(int capacity)
        {
            Stations = new Station[capacity];
            while (capacity-- > 0) Stations[capacity] = new();
        }
        public async Task Damage(
            int stationsNumber,
            int delaySeconds = 1,
            int totalTimeSeconds = 10) =>
            await Stations[stationsNumber].Damage(delaySeconds, totalTimeSeconds);
        public virtual async Task Join(AirplaneModel ap, int time = 5)
        {
            foreach (var s in Stations) await s.Join(ap, time);
        }
    }
}
