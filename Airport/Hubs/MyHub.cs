using Airport.BL;
using Airport.BL.BLIntefeces;
using Airport.Models;
using Airport.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Airport.Hubs
{
    public class MyHub : Hub
    {
        private readonly ITowerControl _towerControl;

        public MyHub(ITowerControl towerControl) => _towerControl = towerControl;

        public async Task SendAirplaneAndStation(StationModel st) =>
            await Clients.Caller.SendAsync("onAirplaneJoin", st);
        public void StartSimulator() => _towerControl.StartSimulator();
        public void DoEmergency(int id) => _towerControl.DoEmergency(id, 12);
    }
}
