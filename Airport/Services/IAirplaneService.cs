using Airport.Models;

namespace Airport.Services
{
    public interface IAirplaneService
    {
        public IEnumerable<Airplane> GetPlanes();
    }
}