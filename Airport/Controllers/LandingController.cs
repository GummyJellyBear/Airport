using Airport.BL;
using Airport.Models;
using Airport.Services;
using Microsoft.AspNetCore.Mvc;

namespace Airport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingController : ControllerBase
    {
        private readonly IAirplaneService _airplaneService;
        private readonly ITowerControl _controlTower;

        public LandingController(
            IAirplaneService airplaneService,
            ITowerControl controlTower)
        {
            _airplaneService = airplaneService;
            _controlTower = controlTower;
        }
        [HttpGet]
        public async Task Start()
        {
            _controlTower.InitiazlizeStations();
            _controlTower.InitiazlizePlanes();
            _controlTower.InitiazAirMovment();
            _controlTower.StartSimulator();
        }
        public async Task DoEmergency()
        {

        }
    }
}
