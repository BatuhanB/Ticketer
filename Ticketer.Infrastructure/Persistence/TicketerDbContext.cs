using Microsoft.EntityFrameworkCore;
using Ticketer.Application.Common.Interfaces;
using Ticketer.Domain.Booking.Aggregates;

namespace Ticketer.Infrastructure.Persistence;
public class TicketerDbContext : DbContext, IUnitOfWork
{
    public DbSet<ShowInventory> ShowInventories => Set<ShowInventory>();

    public TicketerDbContext(DbContextOptions<TicketerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This applies all IEntityTypeConfiguration classes found in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In a fully robust DDD system, you would also intercept this moment 
        // to gather all DomainEvents from your AggregateRoots and publish them 
        // to MediatR before committing the transaction to the database.

        return await base.SaveChangesAsync(cancellationToken);
    }
}
