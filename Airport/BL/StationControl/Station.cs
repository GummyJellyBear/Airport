using Airport.BL.BLIntefeces;
using Airport.Hubs;
using Airport.Models;
using Airport.Services;
using Microsoft.AspNetCore.SignalR;
using System.Xml;

namespace Airport.BL.StationControl
{
    public class Station : StationModel, IStation
    {
        private readonly SemaphoreSlim gate;
        private readonly SemaphoreSlim emergencyGate;
        public Station()
        {
            gate = new SemaphoreSlim(1);
            emergencyGate = new SemaphoreSlim(1);
        }
        public Station(int capacity)
        {
            gate = new SemaphoreSlim(capacity);
            emergencyGate = new SemaphoreSlim(capacity);
        }
        public IHubContext<MyHub>? HubContext { get; set; }
        public int speedPrecent { get; set; } = 300;
        public int Index { get; set; } = 0;

        public async Task Damage(int delaySeconds = 1, int totalTimeSeconds = 10)
        {
            AirplaneInIt = null;
            Thread.Sleep(delaySeconds * 1000);
            await emergencyGate.WaitAsync();
            Console.WriteLine("station : " + StationID + " is in emergencyy");
            Thread.Sleep(totalTimeSeconds * 1000);
            Console.WriteLine("station : " + StationID + " is OK!!!!!!");

            emergencyGate.Release();
        }
        public async Task Join(AirplaneModel ap, int seconds = 5)
        {
            await Entery(ap, seconds);
            await Exit(ap);
        }

        private async Task SendJoinStationToClient(AirplaneModel ap)
        {
            if (HubContext != null)
                await HubContext.Clients.All.SendAsync("onAirplaneJoin",
                    new StationModel { AirplaneInIt = ap, StationID = this.StationID });
        }

        private async Task SendExitStationToClient(AirplaneModel ap)
        {
            if (HubContext != null)
                await HubContext.Clients.All.SendAsync("onAirplaneJoin",
                    new StationModel { AirplaneInIt = null, StationID = this.StationID });
        }
        public async Task Entery(AirplaneModel ap, int seconds = 5)
        {
            await emergencyGate.WaitAsync();
            await gate.WaitAsync();
            await SendJoinStationToClient(ap);
            AirplaneInIt = ap;
            Console.WriteLine(ap.Id + " entered station number " + StationID);
            seconds = seconds * 100 / speedPrecent;
            Thread.Sleep(seconds * 1000);
        }
        bool CheckCrush(AirplaneModel ap)
        {
            if (AirplaneInIt != null)
            {
                Console.WriteLine(" ...BOOMM!!...Crushed!!!!..");
                AirplaneInIt.IsCrashed = true;
                ap.IsCrashed = true;
                return true;
            }
            return false;
        }
        public async Task Exit(AirplaneModel ap)
        {
            AirplaneInIt = null;
            Console.WriteLine(ap.Id + " Exited station number " + StationID);
            gate.Release();
            emergencyGate.Release();
            await SendExitStationToClient(ap);
        }
    }
}
