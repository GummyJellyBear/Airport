using Airport.Models;
using Airport.Services;
using System.Numerics;

namespace Airport.BL
{
    public class ControlLanding : IControlLanding
    {
        public List<Airplane> AirplanesReadyToLand { get; private set; }
            = new List<Airplane>();
        public void SetLandingQueue(Airplane airplane)
        {
            Insert(airplane);
        }
        private void Insert(Airplane airplane)
        {
            var index = AirplanesReadyToLand.FindIndex(a =>
                a.SecondsToArrive > airplane.SecondsToArrive);
            if (index != -1)
                AirplanesReadyToLand.Insert(index, airplane);
            else
                AirplanesReadyToLand.Add(airplane);
        }

    }
}
