using FitnessApi.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<InternalService> InternalServices { get; set; }
        public DbSet<AllowedOrigin> AllowedOrigins { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }

    }
}
