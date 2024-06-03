using DiplomIvanova.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiplomIvanova.DataBase.Context
{
    public class AppDbContext:DbContext
    {
        private readonly string _databasePath = $"{AppContext.BaseDirectory}/DataBase/database.db";
        public DbSet<CarEntity> Cars { get; set; }
        //public DbSet<TripRequestEntity> TripRequests { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<DriverEntity> Drivers { get; set; }
        //public DbSet<ProductEntity> Products { get; set; }
        public DbSet<PickUpPointEntity> PickUpPoints { get; set; }
        public DbSet<RouteEntity> Routes { get; set; }
        public DbSet<TripEntity> Trips { get; set; }

        public AppDbContext()
        {
            SQLitePCL.Batteries.Init();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CarEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ClientEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DriverEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<PickUpPointEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<RouteEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TripEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db");
            optionsBuilder.UseSqlite($"Data Source=database.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
