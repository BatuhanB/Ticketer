using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ticketer.Infrastructure.Persistence
{
    // This class is ONLY used by EF Core tools (like 'dotnet ef migrations add')
    public class TicketerDbContextFactory : IDesignTimeDbContextFactory<TicketerDbContext>
    {
        public TicketerDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TicketerDbContext>();

            // Provide a local connection string purely for generating migrations.
            // This doesn't affect your actual application runtime connection string.
            builder.UseNpgsql("Host=localhost;Port=5432;Database=ticketerdb;Username=postgres;Password=Ticketing@Admin123!");

            return new TicketerDbContext(builder.Options);
        }
    }
}