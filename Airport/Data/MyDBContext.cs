using Airport.BL.StationControl;
using Airport.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Airport.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<AirplaneModel> Airplanes { get; set; }

        public MyDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            {
                Random random = new Random();
                for (int i = 0; i < 100; i++)
                {
                    var newID = random.Next(111, 999);
                    var airp = Airplanes.FirstOrDefault(a => a.Id == newID);

                    if (airp != null) i--;
                    else
                    {
                        modelBuilder.Entity<AirplaneModel>().HasData(
                            new AirplaneModel { Id = newID });
                    }
                }
            }
        }
    }

}
