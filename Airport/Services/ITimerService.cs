namespace Airport.Services
{
    public interface ITimerService
    {
        Task Start(Action timerAction);
        Task StopAsync();
    }
}