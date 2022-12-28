using Airport.Models;
using System;
using System.Timers;

namespace Airport.Services
{
    public class AirplaneService : IAirplaneService
    {
        System.Timers.Timer Timer = new System.Timers.Timer();
        Random random = new Random();
        public AirplaneService()
        {
            InitizPlanes();
        }
        public List<Airplane>? airplanes { get; set; }
        public IEnumerable<Airplane> GetPlanes() => airplanes!;
        public void InitizPlanes()
        {
            airplanes = new List<Airplane>()
            {
                new Airplane() { Id = 1 ,SecondsToArrive = 6},
                new Airplane() { Id = 2 ,SecondsToArrive = 5},
                new Airplane() { Id = 3 ,SecondsToArrive = 4},
                new Airplane() { Id = 4 ,SecondsToArrive = 3},
                new Airplane() { Id = 5 ,SecondsToArrive = 2},
                new Airplane() { Id = 6 ,SecondsToArrive = 1},
                new Airplane() { Id = 7 ,SecondsToArrive = 0}
            };
            Timer.Interval = 1000;
            Timer.Start();
            Timer.Elapsed += TimeMinusOne!;
        }

        private void TimeMinusOne(Object source, ElapsedEventArgs e)
        {
            if (airplanes != null)
                foreach (Airplane airplane in airplanes)
                    airplane.SecondsToArrive--;
        }
    }
}

