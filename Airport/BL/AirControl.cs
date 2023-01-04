using Airport.Models;
using Airport.Services;

namespace Airport.BL
{
    public class AirControl : IAirControl
    {
        private readonly ITimerService _timer;
        public AirControl(ITimerService timer) => _timer = timer;
        public List<Airplane> PlanesToLand { get; set; }
            = new List<Airplane>();
        public async Task FlyTimer(List<Airplane> landingPlanes)
        {
            await _timer.Start(async () =>
            {
                foreach (Airplane airplane in PlanesToLand.ToList())
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
