using Airport.Models;

namespace Airport.BL.StationControl
{
    public class TerminalStations : StationPath
    {
        private readonly SemaphoreSlim gate;
        public TerminalStations(int capacity) : base(capacity)
        {
            gate = new SemaphoreSlim(capacity);
        }
        public async override Task Join(AirplaneModel ap)
        {
            await gate.WaitAsync();
            var st = Stations.FirstOrDefault(a => a.AirplaneInIt == null);
            if(st == null)
                await Stations[0].Join(ap);
            else
                await Stations[st.Index].Join(ap);
            gate.Release();
        }
    }
}
