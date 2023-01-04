using Airport.Models;

namespace Airport.BL
{
    public interface IAirControl
    {
        List<Airplane> PlanesToLand { get; set; }
        Task FlyTimer(List<Airplane> landingPlanes);
    }
}