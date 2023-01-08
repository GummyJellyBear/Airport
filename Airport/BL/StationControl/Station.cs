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

        private readonly SemaphoreSlim gate = new SemaphoreSlim(1);
        private readonly SemaphoreSlim emergencyGate = new SemaphoreSlim(1);
        private readonly HubServiceTask hubServiceTask = new HubServiceTask();


        private static readonly ITimerService _timerService = new TimerService();
        private static readonly IAirplaneService _airplaneService = new AirplaneService(_timerService);


        public IHubContext<MyHub>? HubContext { get; set; }

        public async Task Damage(int delaySeconds = 1, int totalTimeSeconds = 10)
        {
            AirplaneInIt = null;
            Thread.Sleep(delaySeconds * 1000);
            await emergencyGate.WaitAsync();
            Thread.Sleep(totalTimeSeconds * 1000);
            emergencyGate.Release();
        }
        public async Task Join(AirplaneModel ap, int time = 5)
        {
            await Entery(ap);
            Console.WriteLine(ap.Id + " entered station number " + StationID);
            Console.WriteLine(" ...Checking crush...");
            //if (CheckCrush(ap))
            //    return; // change later to remove both airplanes from list
            //await hub.SendAirplaneAndStation(ap, StationID);
            //_airplaneService.SendStation(this);
            //await hub.Clients.All.SendAsync("onAirplaneJoin", new StationModel { AirplaneInIt = ap, StationID = this.StationID });
            //await hub.SendAirplaneAndStation(new StationModel { AirplaneInIt = ap, StationID = this.StationID });
            await SendStationToClient(ap);

            Thread.Sleep(time * 1000);
            Exit();
            Console.WriteLine(ap.Id + " Exited station number " + StationID);
        }

        private async Task SendStationToClient(AirplaneModel ap)
        {
            if (HubContext != null)
                await HubContext.Clients.All.SendAsync("onAirplaneJoin",
                    new StationModel { AirplaneInIt = ap, StationID = this.StationID });
        }

        async Task Entery(AirplaneModel ap)
        {
            AirplaneInIt = ap;
            await emergencyGate.WaitAsync();
            await gate.WaitAsync();
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
        void Exit()
        {
            AirplaneInIt = null;
            gate.Release();
            emergencyGate.Release();
        }
    }
}
