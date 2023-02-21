using Airport.BL.BLIntefeces;
using Airport.BL.StationControl;
using Airport.Hubs;
using Airport.Models;
using Airport.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Airport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly ITowerControl _towerControl;


        public AirportController(ITowerControl towerControl) => _towerControl = towerControl;

        [HttpGet("/start")]
        public void StartSimulator() => _towerControl.StartSimulator();

        [HttpGet("/emergency/{id}")]
        public void Get(int id) => _towerControl.DoEmergency(id, 15);

        [HttpGet("/rain")]
        public void Post()
        {
           
        }
    }
}
