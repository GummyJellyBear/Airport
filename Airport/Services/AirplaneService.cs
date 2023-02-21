using Airport.BL.StationControl;
using Airport.Models;
using System;
using System.Timers;

namespace Airport.Services
{
    public class AirplaneService : IAirplaneService
    {
        private readonly ITimerService _backgroundTimer;
        public AirplaneService(ITimerService timerService)
        {
            InitizPlanes();
            _backgroundTimer = timerService;
        }
        public List<AirplaneModel> PlanesOnPath { get; private set; }
          = new List<AirplaneModel>();
        public List<AirplaneModel> PlanesParking { get; private set; }
          = new List<AirplaneModel>();
        public List<AirplaneModel> Airplanes { get; set; } = new List<AirplaneModel>();
        public List<StationModel> Stations { get; set; } = new List<StationModel>();
        public void InitizPlanes()
        {
            Airplanes = new List<AirplaneModel>()
            {
                new AirplaneModel() { Id = 1 ,SecondsToArrive = 12},
                new AirplaneModel() { Id = 2 ,SecondsToArrive = 10},
                new AirplaneModel() { Id = 3 ,SecondsToArrive = 8},
                new AirplaneModel() { Id = 4 ,SecondsToArrive = 6},
                new AirplaneModel() { Id = 5 ,SecondsToArrive = 4},
                new AirplaneModel() { Id = 6 ,SecondsToArrive = 2},
                new AirplaneModel() { Id = 7 ,SecondsToArrive = 0}
            };
            Airplanes.ForEach(airP => Console.WriteLine("plane id:" + airP.Id + " time: " + airP.SecondsToArrive));

            PlanesParking = new List<AirplaneModel>()
            {
                new AirplaneModel() { Id = 8 ,SecondsToArrive = null },
                new AirplaneModel() { Id = 9 ,SecondsToArrive = null },
                new AirplaneModel() { Id = 10 ,SecondsToArrive = null },
                new AirplaneModel() { Id = 11 ,SecondsToArrive = null },
                new AirplaneModel() { Id = 12 ,SecondsToArrive = null },
                new AirplaneModel() { Id = 13 ,SecondsToArrive = null },
                new AirplaneModel() { Id = 14 ,SecondsToArrive = null }
            };
        }
        public async Task QueueAirplanes(List<AirplaneModel> list, Action<AirplaneModel> action)
        {
            await _backgroundTimer.Start(() =>
            {
                if (list.Count != 0)
                {
                    var plane = list[0];
                    list.RemoveAt(0);
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        action.Invoke(plane);
                    }));
                    thread.Start();
                }
            });
        }
        public void SendStation(Station ap) => Stations.Add(ap);
    }
}

