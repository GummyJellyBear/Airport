using Airport.Hubs;
using Airport.Models;
using Airport.Models.Enum;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Reflection;

namespace Airport.BL
{
    public class PathControl : IPathControl
    {
        private readonly IHubContext<MyHub> _hub;
        public PathControl(IHubContext<MyHub> hub) => _hub = hub;
        const int airGatesLength = 3;
        const int toTerminalGatesLength = 2;
        const int terminalGatesLength = 2;
        const int fromTerminalGatesLength = 2;

        readonly SemaphoreSlim airRadiusGate = new SemaphoreSlim(1);//0
        readonly SemaphoreSlim[] airGates = new SemaphoreSlim[airGatesLength];//1
        readonly SemaphoreSlim sharedGate = new SemaphoreSlim(1);//2 + //6 (shared)
        readonly SemaphoreSlim[] toTerminalGates = new SemaphoreSlim[toTerminalGatesLength];//3
        readonly SemaphoreSlim terminalGates = new SemaphoreSlim(terminalGatesLength);//4
        readonly SemaphoreSlim[] fromTerminalGates = new SemaphoreSlim[fromTerminalGatesLength];//5

        readonly bool[] isTerminalGatesUnavailable = new bool[terminalGatesLength];//4

        readonly SemaphoreSlim isAirRadiusGateInEmergency = new SemaphoreSlim(1);//0
        readonly SemaphoreSlim[] isAirGatesInEmergency = new SemaphoreSlim[airGatesLength];//1
        readonly SemaphoreSlim isSharedGateInEmergency = new SemaphoreSlim(1);//2 + //6 (shared)
        readonly SemaphoreSlim[] isToTerminalGatesInEmergency = new SemaphoreSlim[toTerminalGatesLength];//3
        readonly bool[] isTerminalGatesInEmergency = new bool[terminalGatesLength];//4
        readonly SemaphoreSlim[] isfromTerminalGatesInEmergency = new SemaphoreSlim[toTerminalGatesLength];//5

        //sharedGate is for the station that planes land on or to takeoff from.
        /// <summary>
        /// all of the countdowns, waiting for paths to be available,
        /// enetring the terminal, waitng for travelers to get in the plane
        /// </summary>
        public List<Airplane> PlanesOnPath { get; private set; }
           = new List<Airplane>();
        public void InitiazlizeStations()
        {
            for (int i = 0; i < airGatesLength; i++)
            {
                airGates[i] = new SemaphoreSlim(1);
            }
            for (int i = 0; i < toTerminalGatesLength; i++)
            {
                toTerminalGates[i] = new SemaphoreSlim(1);
            }
            for (int i = 0; i < fromTerminalGatesLength; i++)
            {
                fromTerminalGates[i] = new SemaphoreSlim(1);
            }
        }
        public async Task LandToTakeoff(int airplaneID)
        {
            Console.WriteLine(">>>>-----New-----Plane-----Enter-------->>>>");
            await AirRadiusStepIn(airplaneID);
            await AirPathAsync(airplaneID);//1
            Land(airplaneID);//2
            await ToTermianlPathAsync(airplaneID);//3
            await TermianlPathAsync(airplaneID);//4
            await FromTermianlPathAsync(airplaneID);//5
            TakeOffAsync(airplaneID);//6
            Console.WriteLine("<<<<<---" + airplaneID + "---Exited---Airport---Area---<<<<");
        }
        public async Task DoEmergency(StationType stationType, int stationNumber, int SOSSeconds)
        {
            switch (stationType)
            {
                case StationType.AirStation:
                    await airGates[stationNumber].WaitAsync();
                    Thread.Sleep(SOSSeconds * 1000);
                    break;
                case StationType.LandOrTakeoff:
                    await sharedGate.WaitAsync();
                    Thread.Sleep(SOSSeconds * 1000);
                    break;
                case StationType.ToTerminal:
                    await isToTerminalGatesInEmergency[stationNumber].WaitAsync();
                    Thread.Sleep(SOSSeconds * 1000);
                    break;
                case StationType.Terminal:
                    isTerminalGatesInEmergency[stationNumber] = true;
                    Thread.Sleep(SOSSeconds * 1000);
                    break;
                case StationType.FromTerminal:
                    await isfromTerminalGatesInEmergency[stationNumber].WaitAsync();
                    Thread.Sleep(SOSSeconds * 1000);
                    break;
            }
        }
        async Task AirRadiusStepIn(int airplaneID)
        {
            await _hub.Clients.All.SendAsync("onAirRadiusStepIn");
            Console.WriteLine(airplaneID + " is in the sky and want to land");
            Thread.Sleep(15_000);
            await isAirRadiusGateInEmergency.WaitAsync();
            isAirRadiusGateInEmergency.Release();
            await airRadiusGate.WaitAsync();
            airRadiusGate.Release();
        }
        async Task AirPathAsync(int airplaneID)
        {
            var index = 0;
            await isSharedGateInEmergency.WaitAsync();
            isSharedGateInEmergency.Release();
            await sharedGate.WaitAsync();
            while (index < airGates.Length)
            {
                await _hub.Clients.All.SendAsync("onAirPathAsync" + index);
                await isAirGatesInEmergency[index].WaitAsync();
                isAirGatesInEmergency[index].Release();
                await airGates[index].WaitAsync();
                Console.WriteLine
                (
                    airplaneID +
                    " landing within " +
                    (airGates.Length - index) * 1000 +
                    "Caliometers"
                );
                Thread.Sleep(3000);
                airGates[index].Release();
                index++;
            }
        }//1
        async Task LandAsync(int airplaneID)
        {
            await _hub.Clients.All.SendAsync("onLandAsync");
            Console.WriteLine(airplaneID + " IS LANDING ");
            Thread.Sleep(6000);
            Console.WriteLine(airplaneID + " LANDED SUCCESSFULLY ");
        }//2
        async Task ToTermianlPathAsync(int airplaneID)
        {
            var index = 0;
            while (index < toTerminalGates.Length)
            {
                await _hub.Clients.All.SendAsync("onToTermianlPathAsync"+index);
                await isToTerminalGatesInEmergency[index].WaitAsync();
                isToTerminalGatesInEmergency[index].Release();
                await toTerminalGates[index].WaitAsync();
                Console.WriteLine(airplaneID + " moving towards the terminal");
                Thread.Sleep(2000);
                toTerminalGates[index].Release();
                // one station after airplane on the ground
                if (index == 1) sharedGate.Release();
                index++;
            }
        }//3
        async Task TermianlPathAsync(int airplaneID)
        {
            bool isNoEntery = true;
            await terminalGates.WaitAsync();
            do
            {
                for (int i = 0; i < isTerminalGatesUnavailable.Length; i++)
                {
                    if (!isTerminalGatesUnavailable[i] && !isTerminalGatesInEmergency[i])
                    {
                        await _hub.Clients.All.SendAsync("onTermianlPathAsync" + i);
                        isNoEntery = false;
                        isTerminalGatesUnavailable[i] = true;
                        Console.WriteLine
                        (
                            airplaneID +
                            " Entered the terminal. station number: " +
                            i
                        );
                        Thread.Sleep(25_000);
                        terminalGates.Release();
                        break;
                    }
                }
                Thread.Sleep(5000);
            } while (isNoEntery);
        }//4
        async Task FromTermianlPathAsync(int airplaneID)
        {
            var index = 0;
            while (index < fromTerminalGates.Length)
            {
                await _hub.Clients.All.SendAsync("onTermianlPathAsync" + index);
                await isfromTerminalGatesInEmergency[index].WaitAsync();
                isfromTerminalGatesInEmergency[index].Release();
                await fromTerminalGates[index].WaitAsync();
                Console.WriteLine
                (
                    airplaneID +
                    " moving to takeoff... " +
                    index
                );
                Thread.Sleep(4000);
                if (index == fromTerminalGates.Length - 1)
                    await sharedGate.WaitAsync();
                fromTerminalGates[index].Release();
                index++;
            }
        }//5
        async Task TakeOffAsync(int airplaneID)
        {
            await _hub.Clients.All.SendAsync("onTermianlPathAsync");
            Console.WriteLine(airplaneID + " IS TAKINGOFF ");
            Thread.Sleep(6000);
            Console.WriteLine(airplaneID + " TAKEOFF SUCCESSFULLY ");
            sharedGate.Release();
        }//6
    }
}