using Airport.BL.BLIntefeces;
using Airport.Models;
using Airport.Services;

namespace Airport.BL
{
    public class AirControl : IAirControl
    {
        private readonly ITimerService _timer;
        public AirControl(ITimerService timer) => _timer = timer;
        public List<AirplaneModel> PlanesToLand { get; set; }
            = new List<AirplaneModel>();
        public async Task FlyTimer(List<AirplaneModel> landingPlanes)
        {
            await _timer.Start(() =>
            {
                foreach (AirplaneModel airplane in PlanesToLand.ToList())
                {
                    airplane.SecondsToArrive--;
                    if (airplane.SecondsToArrive <= 0)
                    {
                        PlanesToLand.Remove(airplane);
                        landingPlanes.Add(airplane);
                        airplane.SecondsToArrive = null;
                        Console.WriteLine("Plane num:" + airplane.Id + " wants to land");
                    }
                }
            });
        }
    }
}
