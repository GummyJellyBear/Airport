namespace Airport.BL
{
    public interface IControlTower
    {
        void InitiazlizePlanes();
        void InitiazlizeStations();
        Task LandToTakeoff(int airplaneID);
        void StartSimulator();
    }
}