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

        // GET: api/<TestingAirportController>
        [HttpGet]
        public void StartSimulator()
        {
            _towerControl.StartSimulator();
            //_hub.Clients.All.SendAsync("testing", "hello");
            
        }

        // GET api/<TestingAirportController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestingAirportController>
        [HttpPost]
        public void Post()
        {
           
        }

        // PUT api/<TestingAirportController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestingAirportController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
