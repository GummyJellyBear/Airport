using Airport.Hubs;
using Airport.Models;
using Microsoft.AspNetCore.SignalR;

namespace Airport.BL.StationControl
{
    public interface IStation
    {
        public int Index { get; set; }
        public int speedPrecent { get; set; }
        public IHubContext<MyHub>? HubContext { get; set; }
        Task Damage(int delaySeconds = 1, int totalTimeSeconds = 10);
        Task Join(AirplaneModel ap, int seconds = 5);
    }
}