using Microsoft.EntityFrameworkCore;
using SMApi.Models;


namespace SMApi.Data
{
    public class SMApiContext : DbContext
    {

        public SMApiContext (DbContextOptions<SMApiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }

        public DbSet<Desk> Desks => Set<Desk>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<User> Users => Set<User>(); 

    }
}
