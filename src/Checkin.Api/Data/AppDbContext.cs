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
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}