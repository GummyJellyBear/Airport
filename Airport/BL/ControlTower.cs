using Airport.Models;
using Airport.Services;

namespace Airport.BL
{
    public class ControlTower : IControlTower
    {
        Station[] stations = new Station[4];
        readonly SemaphoreSlim[] gates = new SemaphoreSlim[4];
        readonly IEnumerable<Airplane> airplanes;
        private readonly IAirplaneService _airplaneService;
        private readonly IControlLanding _controlLanding;

        public ControlTower(
            IAirplaneService airplaneService,
            IControlLanding controlLanding)
        {
            _airplaneService = airplaneService;
            _controlLanding = controlLanding;
            airplanes = _airplaneService.GetPlanes();
        }
        public void InitiazlizeStations()
        {
            for (int i = 0; i < 4; i++)
            {
                stations[i] = new Station() { Id = i };
                gates[i] = new SemaphoreSlim(1);
            }
        }
        public void InitiazlizePlanes()
        {
            foreach (Airplane plane in _airplaneService.GetPlanes())
                _controlLanding.SetLandingQueue(plane);
            foreach (Airplane plane in _controlLanding.AirplanesReadyToLand)
                Console.WriteLine("Airplane Time:" + plane.SecondsToArrive + "  Airplane number:" + plane.Id);
        }
        public void StartSimulator()
        {
            //Parallel.For(0, _controlLanding.AirplanesReadyToLand.Count, async (i) =>
            //{
            //    await LandToTakeoff(_controlLanding.AirplanesReadyToLand[i].Id);
            //});
            foreach (var plane in _controlLanding.AirplanesReadyToLand)
                if (plane.SecondsToArrive <= 1) // when 1 second to land, land
                {
                    //Parallel.Invoke
                    //(
                    //    async () => await LandToTakeoff(plane.Id)
                    //);
                    new Thread(async () =>
                    {
                        await LandToTakeoff(plane.Id);
                    }).Start();
                }
        }
        public async Task LandToTakeoff(int airplaneID)
        {
            var count = 1;
            while (count < 4)
            {
                await gates[count].WaitAsync();
                Console.WriteLine(airplaneID + " Entered station number: " + count);
                Console.WriteLine();
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(1000);
                    Console.Write(".");
                }
                Console.WriteLine();
                TryEnterStation(airplaneID, count);
                Console.WriteLine(airplaneID + " have Exited station number: " + count);
                gates[count].Release();
                count++;
            }
            Console.WriteLine(airplaneID + " take off");
        }
        private void TryEnterStation(int airplaneId, int count)
        {
            if (stations[count].AirplaneId == null)
                stations[count].AirplaneId = airplaneId;
            if (stations[count].AirplaneId != null)
                stations[count].AirplaneId = null;
        }
    }
}
