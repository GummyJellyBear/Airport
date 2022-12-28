using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Airport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingControll : ControllerBase
    {
        SemaphoreSlim gate = new SemaphoreSlim(3);
        [HttpPost]
        public void Test()
        {
            Parallel.Invoke(
                () => DoWork(),
                () => DoWork()
            );
        }
        private async Task DoWork()
        {
            for (int i = 0; i < 10; i++)
            {
                await gate.WaitAsync();
                Thread.Sleep(2000);
                Console.WriteLine(i);
                gate.Release();
            }
        }
    }
}
