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
        }
        public void SortAirplanes(List<AirplaneModel> listOnPath, Action<AirplaneModel> action)
        {
            _backgroundTimer.Start(() =>
            {
                if (listOnPath.Count != 0)
                {
                    var plane = listOnPath[0];
                    listOnPath.RemoveAt(0);
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        action.Invoke(plane);
                    }));
                    thread.Start();
                }
            });
        }
        public void SendStation(Station ap)
        {
            Stations.Add(ap);
        }

    }
}

