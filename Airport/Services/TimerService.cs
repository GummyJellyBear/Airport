namespace Airport.Services
{
    public class TimerService : ITimerService
    {
        private PeriodicTimer _timer;
        private Action _timerAction;
        private Task? _timerTask;
        private readonly CancellationTokenSource _cts = new();

        public async Task Start(Action timerAction)
        {
            _timer = new PeriodicTimer(new TimeSpan(0, 0, 1));
            _timerAction = timerAction;
            _timerTask = DoWorkAsync();
        }
        private async Task DoWorkAsync()
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    _timerAction.Invoke();
                }
            }
            catch (OperationCanceledException) { }
        }
        public async Task StopAsync()
        {
            if (_timerTask == null)
            {
                return;
            }
            _cts.Cancel();
            await _timerTask;
            _cts.Dispose();
            Console.WriteLine("TIMER STOPED");
        }
    }
}
