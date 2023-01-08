namespace Airport.BL.BLIntefeces
{
    public interface ITowerControl
    {
        void StartSimulator();
        public void DoEmergency(int stationNumber, int SOSSeconds);
    }
}