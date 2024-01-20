using Microsoft.EntityFrameworkCore;
using SimbirSoft.Models;

namespace SimbirSoft.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public ApplicationDbContext() { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<VisitedLocation> VisitedLocations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=animal-chipization;Username=user;Password=password");
            optionsBuilder.UseSqlite("Filename = Database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
