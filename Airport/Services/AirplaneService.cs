using Airport.Models;
using System;
using System.Timers;

namespace Airport.Services
{
    public class AirplaneService : IAirplaneService
    {
        public AirplaneService() => InitizPlanes();
        public List<Airplane>? airplanes { get; set; }
        public IEnumerable<Airplane> GetPlanes() => airplanes!;
        public void InitizPlanes()
        {
            airplanes = new List<Airplane>()
            {
                new Airplane() { Id = 1 ,SecondsToArrive = 12},
                new Airplane() { Id = 2 ,SecondsToArrive = 10},
                new Airplane() { Id = 3 ,SecondsToArrive = 8},
                new Airplane() { Id = 4 ,SecondsToArrive = 6},
                new Airplane() { Id = 5 ,SecondsToArrive = 4},
                new Airplane() { Id = 6 ,SecondsToArrive = 2},
                new Airplane() { Id = 7 ,SecondsToArrive = 0}
            };
            airplanes.ForEach(airP => Console.WriteLine("plane id:" + airP.Id + " time: " + airP.SecondsToArrive));
        }
    }
}

