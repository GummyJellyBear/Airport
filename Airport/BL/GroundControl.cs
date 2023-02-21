using Airport.BL.BLIntefeces;
using Airport.Models;
using Airport.Services;

namespace Airport.BL
{
    public class GroundControl : IGroundControl
    {
        private readonly ITimerService _timer;
        public GroundControl(ITimerService timer) => _timer = timer;
        public List<AirplaneModel> PlanesToTakeoff { get; set; }
            = new List<AirplaneModel>();
        public async Task MoveTimer(List<AirplaneModel> takeoffPlanes)
        {
            await _timer.Start(() =>
            {
                foreach (AirplaneModel airplane in PlanesToTakeoff.ToList())
                {
                    airplane.SecondsToArrive--;
                    if (airplane.SecondsToArrive <= 0 || airplane.SecondsToArrive == null)
                    {
                        PlanesToTakeoff.Remove(airplane);
                        takeoffPlanes.Add(airplane);
                        Console.WriteLine("Plane num:" + airplane.Id + " wants to takoff");
                    }
                }
            });
        }
    }
}
