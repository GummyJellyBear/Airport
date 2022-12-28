using Airport.Models;

namespace Airport.BL
{
    public class ControlStation
    {
        static readonly SemaphoreSlim gate = new SemaphoreSlim(1);
        public static async Task TryEnter()
        {
            await gate.WaitAsync();
            Thread.Sleep(3000);
        }
        public static void Leave()
        {
            gate.Release();
        }
    }
}
