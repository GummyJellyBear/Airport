using Airport.Models;

namespace Airport.BL.BLIntefeces
{
    public interface IAirControl
    {
        List<AirplaneModel> PlanesToLand { get; set; }
        Task FlyTimer(List<AirplaneModel> landingPlanes);
    }
}