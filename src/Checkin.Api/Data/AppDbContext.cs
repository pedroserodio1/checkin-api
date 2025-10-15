using Microsoft.EntityFrameworkCore;
using Checkin.Api.Models;

/* The `namespace Checkin.Api.Data` statement in the C# code is declaring a namespace for the classes
defined within it. Namespaces are used to organize code and prevent naming conflicts. In this case,
the `AppDbContext` class and other related classes within the `Checkin.Api.Data` namespace will be
grouped together under this namespace. This helps in organizing and structuring the codebase in a
logical manner. */
namespace Checkin.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
             .Property(u => u.RegisteredAt)
             .HasDefaultValueSql("CURRENT_TIMESTAMP");


        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Event &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (Event)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}