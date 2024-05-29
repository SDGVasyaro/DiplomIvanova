using DiplomIvanova.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiplomIvanova.DataBase.Context
{
    public class AppDbContext:DbContext
    {
        
        public DbSet<CarEntity> Cars { get; set; }
        //public DbSet<TripRequestEntity> TripRequests { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<DriverEntity> Drivers { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<PickUpPointEntity> PickUpPoints { get; set; }
        public DbSet<RouteEntity> Routes { get; set; }
        //public DbSet<StorageEntity> RouteIntermediatePoints { get; set; }
        public DbSet<TripEntity> Trips { get; set; }

        public AppDbContext()
        {
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
		{
			//foreach (var entry in ChangeTracker.Entries<IEntityBase>())
			//{
			//	var dateNow = DateTime.Now;
			//	if (entry.State is EntityState.Added)
			//	{
   //                 entry.Entity.CreatedAt = dateNow;
   //                 entry.Entity.UpdatedAt = dateNow;
   //             }
			//	else if (entry.State is EntityState.Modified)
			//	{
			//		entry.Entity.UpdatedAt = dateNow;
			//	}
			//}

			return await base.SaveChangesAsync(cancellationToken);
		}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            
            modelBuilder.UseIdentityByDefaultColumns();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MauiProgram).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=diplomDb;Username=postgres;Password=666");
        }
    }
}
