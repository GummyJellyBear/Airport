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
        public async override Task Join(AirplaneModel ap, int time = 5)
        {
            var st = Stations.FirstOrDefault(a => a.AirplaneInIt == null);
            if(st == null)
                await Stations[0].Join(ap, time);
            else
                await Stations[st.StationID].Join(ap, time);
            

        }
    }
}
