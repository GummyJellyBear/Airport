using Airport.BL.BLIntefeces;
using Airport.BL.StationControl;
using Airport.Hubs;
using Airport.Models;
using Airport.Services;
using Microsoft.AspNetCore.SignalR;

namespace Airport.BL
{
    public class PathControl : IPathControl
    {

        readonly Station[] allStations = new Station[11];
        readonly Station airRaduisEnteryStation;
        readonly StationPath airStations;
        readonly Station sharedStation;
        readonly StationPath toTerminalStations;
        readonly TerminalStations terminalStation;
        readonly StationPath fromTerminalStations;
        const int airGatesLength = 3;
        const int toTerminalGatesLength = 2;
        const int terminalGatesLength = 2;
        const int fromTerminalGatesLength = 2;

        //sharedGate is for the station that planes land on or to takeoff from.
        /// <summary>
        /// all of the countdowns, waiting for paths to be available,
        /// enetring the terminal, waitng for travelers to get in the plane
        /// </summary>
        private readonly IAirplaneService _airplaneService;

        public PathControl(IAirplaneService airplaneService)
        {
            _airplaneService = airplaneService;
            airRaduisEnteryStation = new();
            airStations = new(airGatesLength);
            sharedStation = new();
            toTerminalStations = new(toTerminalGatesLength);
            terminalStation = new(terminalGatesLength);
            fromTerminalStations = new(fromTerminalGatesLength);
        }


        public List<AirplaneModel> PlanesOnPath { get; private set; }
           = new List<AirplaneModel>();
        public void InitiazlizeStations(IHubContext<MyHub> context)
        {
            allStations[0] = airRaduisEnteryStation;
            allStations[1] = airStations.Stations[0];
            allStations[2] = airStations.Stations[1];
            allStations[3] = airStations.Stations[2];
            allStations[4] = sharedStation;
            allStations[5] = toTerminalStations.Stations[0];
            allStations[6] = toTerminalStations.Stations[1];
            allStations[7] = terminalStation.Stations[0];
            allStations[8] = terminalStation.Stations[1];
            allStations[9] = fromTerminalStations.Stations[0];
            allStations[10] = fromTerminalStations.Stations[1];
            var indexer = 0;
            foreach(var s in allStations)
            {
                allStations[indexer].StationID = indexer;
                allStations[indexer].HubContext = context;
                indexer++;
            }
        }
        public async Task LandToTakeoff(AirplaneModel ap)
        {
            Console.WriteLine(">>>>-----New-----Plane-----Enter-------->>>>");
            await airRaduisEnteryStation.Join(ap);
            await airStations.Join(ap);
            await sharedStation.Join(ap);
            await toTerminalStations.Join(ap);
            await terminalStation.Join(ap);
            await fromTerminalStations.Join(ap);
            await sharedStation.Join(ap);
            Console.WriteLine("<<<<<---" + ap.Id + "---Exited---Airport---Area---<<<<");
        }
        public async Task Land(AirplaneModel ap)
        {
            Console.WriteLine(">>>>-----New-----Plane-----Enter-------->>>>");
            await airRaduisEnteryStation.Join(ap);
            await airStations.Join(ap);
            await sharedStation.Join(ap);
            await toTerminalStations.Join(ap);
            await terminalStation.Join(ap);
            // add to Parking later
            Console.WriteLine(">>>>>---" + ap.Id + "---Parcking---At---Airport--->>>>>>");
        }
        public async Task Takeoff(AirplaneModel ap)
        {
            Console.WriteLine(">>>>-----New-----Plane-----Enter-------->>>>");
            await fromTerminalStations.Join(ap);
            await sharedStation.Join(ap);
            Console.WriteLine("<<<<<---" + ap.Id + "---Exited---Airport---Area---<<<<");
            // add to Parking later
        }
        public async Task DoEmergency(int stationNumber, int SOSSeconds)
        {

        }




    }
}