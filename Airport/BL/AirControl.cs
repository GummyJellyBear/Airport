using Airport.Models;

namespace Airport.BL
{
    public class AirControl
    {
        System.Timers.Timer Timer;
        public AirControl()
        {
            Timer = new System.Timers.Timer();
            Timer.Interval = 1000;
        }
        public async Task AirTimer(Airplane airplane)
        {

        }
    }
}
