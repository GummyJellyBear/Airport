using Airport.Models;

namespace Airport.BL.BLIntefeces
{
    public interface IGroundControl
    {
        List<AirplaneModel> PlanesToTakeoff { get; set; }

        Task MoveTimer(List<AirplaneModel> takeoffPlanes);
    }
}