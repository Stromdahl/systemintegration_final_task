using Microsoft.EntityFrameworkCore;

namespace VehicleHotSpotBackend.Web.Models
{
    public class DrivingDataContext : DbContext
    {
        public DrivingDataContext(DbContextOptions<DrivingDataContext> options) : base(options) {}

        public DbSet<DrivingDataItem> DrivingDataItems { get; set; } = null!;

    }
}