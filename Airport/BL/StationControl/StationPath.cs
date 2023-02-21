using Airport.Hubs;
using Airport.Models;
using Airport.Services;
using Microsoft.AspNetCore.SignalR;

namespace Airport.BL.StationControl
{
    public class StationPath
    {
        public Station[] Stations { get; set; }
        public StationPath(int capacity)
        {
            Stations = new Station[capacity];
            for (int i = 0; i < Stations.Length; i++)
            {
                Stations[i] = new() { Index = i};
            }
        }
        public async Task Damage(
            int stationsNumber,
            int delaySeconds = 1,
            int totalTimeSeconds = 10) =>
            await Stations[stationsNumber].Damage(delaySeconds, totalTimeSeconds);
        public virtual async Task Join(AirplaneModel ap)
        {
            foreach (var s in Stations) await s.Join(ap);
        }
    }
}
