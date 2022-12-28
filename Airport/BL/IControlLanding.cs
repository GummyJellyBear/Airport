using Airport.Models;

namespace Airport.BL
{
    public interface IControlLanding
    {
        List<Airplane> AirplanesReadyToLand { get; }

        void SetLandingQueue(Airplane airplane);
    }
}