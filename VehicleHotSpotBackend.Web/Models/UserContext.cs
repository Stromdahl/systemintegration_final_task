using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace VehicleHotSpotBackend.Web.Models
{
    public class UserContext: DbContext
    {
       public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        public DbSet<UserItem> UserItems { get; set; } = null!;
    }
}
