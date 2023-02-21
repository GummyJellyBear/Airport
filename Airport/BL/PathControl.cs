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
        readonly Station airplaneEntery;
        readonly Station terminalEntery;

        
        readonly Station[] landTakeoffPath = new Station[11];
        readonly Station[] terminal = new Station[2];
        readonly Station[] landPath = new Station[11];
        readonly Station[] takeoffPath = new Station[11];

        readonly Station airRaduisStation;
        readonly Station airStation1;
        readonly Station airStation2;
        readonly Station airStation3;
        readonly Station sharedStation;
        readonly Station toTerminal1;
        readonly Station toTerminal2;
        readonly Station terminalStation1;
        readonly Station terminalStation2;
        readonly Station fromTerminal1;
        readonly Station fromTerminal2;

        //sharedGate is for the station that planes land on or to takeoff from.
        /// <summary>
        /// all of the countdowns, waiting for paths to be available,
        /// enetring the terminal, waitng for travelers to get in the plane
        /// </summary>
        private readonly IAirplaneService _airplaneService;

        public PathControl(IAirplaneService airplaneService)
        {
            _airplaneService = airplaneService;
            airplaneEntery = new();
            terminalEntery = new(2);

            airRaduisStation = new();
            airStation1 = new();
            airStation2 = new();
            airStation3 = new();
            sharedStation = new();
            toTerminal1 = new();
            toTerminal2 = new();
            terminalStation1 = new();
            terminalStation2 = new();
            fromTerminal1 = new();
            fromTerminal2 = new();
        }


        public List<AirplaneModel> PlanesOnPath { get; private set; }
           = new List<AirplaneModel>();
        public void InitiazlizeStations(IHubContext<MyHub> context)
        {
            SetLandToTakeOffPath(context);
            SetLandPath();
            SetTakeOffPath();
            SetTermianlPath();
        }

        public async Task DoEmergency(int stationNumber, int SOSSeconds)
        {
            await landTakeoffPath[stationNumber].Damage(1, SOSSeconds);
        }
        public void DoRain(int time)
        {
            foreach (var station in landTakeoffPath)
                station.speedPrecent = 75; // 75%
            Thread.Sleep(time * 100);
            foreach (var station in landTakeoffPath)
                station.speedPrecent = 100; // 100%
        }
        public async Task Land(AirplaneModel ap)
        {
            Console.WriteLine(">>>>-----New-----Plane-----Enter-------->>>>");

            await JoinAirport(ap);

            var st = await EnterTerminal(ap);
            await ExitTerminal(ap, st);
        }
        public async Task Takeoff(AirplaneModel ap)
        {
            var st = await EnterTerminal(ap);

            await ExitAirport(ap, st);

            Console.WriteLine("<<<<<---" + ap.Id + "---Exited---Airport---Area---<<<<");
        }
        public async Task LandToTakeoff(AirplaneModel ap)
        {
            Console.WriteLine(">>>>-----New-----Plane-----Enter-------->>>>");

            await JoinAirport(ap);

            var st = await EnterTerminal(ap);

            await ExitAirport(ap, st);

            Console.WriteLine("<<<<<---" + ap.Id + "---Exited---Airport---Area---<<<<");
        }

        //////////////////////////////////////////////////////////////////////////////////
        async Task JoinAirport(AirplaneModel ap)
        {
            //event sourcing 
            //event driven/ broker 
            await airplaneEntery.Entery(ap);
            Console.WriteLine(">>>>-----New-----Plane-----Enter-------->>>>");
            await airRaduisStation.Entery(ap);
            await airStation1.Join(ap);
            await airStation2.Join(ap);
            await airStation3.Join(ap);
            await sharedStation.Entery(ap, 8); // landing
            await toTerminal1.Entery(ap, 10);
            await airRaduisStation.Exit(ap);
            await airplaneEntery.Exit(ap);
            await sharedStation.Exit(ap); // landing
            await toTerminal2.Entery(ap, 14);
            await toTerminal1.Exit(ap);
            Console.WriteLine(">>>>>---" + ap.Id + "---Parcking---At---Airport--->>>>>>");
        }
        async Task<Station?> EnterTerminal(AirplaneModel ap)
        {
            await terminalEntery.Entery(ap, 1);
            //if (toTerminal2.AirplaneInIt == null)
            await toTerminal2.Exit(ap);

            var st = terminal.FirstOrDefault(a => a.AirplaneInIt == null);

            if (st == null) await landTakeoffPath[7].Entery(ap, 50);
            else await landTakeoffPath[st.StationID].Entery(ap, 50);

            return st;
        }
        async Task ExitAirport(AirplaneModel ap, Station? terminalStation)
        {
            await fromTerminal1.Entery(ap);
            await ExitTerminal(ap, terminalStation);
            await fromTerminal2.Entery(ap);
            await fromTerminal1.Exit(ap);
            await airRaduisStation.Entery(ap);
            await sharedStation.Entery(ap); // takeoff
            await fromTerminal2.Exit(ap);
            await sharedStation.Exit(ap); // takeoff
            await airRaduisStation.Exit(ap);
            Console.WriteLine("<<<<<---" + ap.Id + "---Takeoff---And---Exit---Airport---Area----<<<<");
        }
        async Task ExitTerminal(AirplaneModel ap, Station? st)
        {
            if (st == null) await landTakeoffPath[0].Exit(ap);
            else await landTakeoffPath[st.StationID].Exit(ap);
            await terminalEntery.Exit(ap);
        }

        void SetLandToTakeOffPath(IHubContext<MyHub> context)
        {
            landTakeoffPath[0] = airRaduisStation;
            landTakeoffPath[1] = airStation1;
            landTakeoffPath[2] = airStation2;
            landTakeoffPath[3] = airStation3;
            landTakeoffPath[4] = sharedStation;
            landTakeoffPath[5] = toTerminal1;
            landTakeoffPath[6] = toTerminal2;
            landTakeoffPath[7] = terminalStation1;
            landTakeoffPath[8] = terminalStation2;
            landTakeoffPath[9] = fromTerminal1;
            landTakeoffPath[10] = fromTerminal2;

            var indexer = 0;
            foreach (var s in landTakeoffPath)
            {
                landTakeoffPath[indexer].StationID = indexer;
                landTakeoffPath[indexer].HubContext = context;
                indexer++;
            }
        }
        void SetLandPath()
        {
            landPath[0] = airRaduisStation;
            landPath[1] = airStation1;
            landPath[2] = airStation2;
            landPath[3] = airStation3;
            landPath[4] = sharedStation;
            landPath[5] = toTerminal1;
            landPath[6] = toTerminal2;
            landPath[7] = terminalStation1;
            landPath[8] = terminalStation2;
        }
        void SetTakeOffPath()
        {
            takeoffPath[0] = terminalStation1;
            takeoffPath[1] = terminalStation2;
            takeoffPath[2] = fromTerminal1;
            takeoffPath[3] = fromTerminal2;
            takeoffPath[4] = sharedStation;
        }
        void SetTermianlPath()
        {
            terminal[0] = terminalStation1;
            terminal[1] = terminalStation2;
        }
    }
}