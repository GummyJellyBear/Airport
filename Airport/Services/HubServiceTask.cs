using Airport.Models;

namespace Airport.Services
{
    public class HubServiceTask
    {
        public StationModel MyStation { get; set; }
        public async Task GetStationToUpdate(StationModel station)
        {
            TaskCompletionSource<StationModel> accessStationTaskSource = new TaskCompletionSource<StationModel>();
            new Thread(() =>
            {
                accessStationTaskSource.TrySetResult(station);
            }).Start();

            MyStation = await accessStationTaskSource.Task;
        }
    }
}
