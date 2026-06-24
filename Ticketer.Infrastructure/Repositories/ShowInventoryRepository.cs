using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticketer.Domain.Booking;
using Ticketer.Domain.Booking.Aggregates;
using Ticketer.Infrastructure.Persistence;

namespace Ticketer.Infrastructure.Repositories;

// Concrete implementation of the domain interface
public class ShowInventoryRepository : IShowInventoryRepository
{
    private readonly TicketerDbContext _context;

    public ShowInventoryRepository(TicketerDbContext context)
    {
        _context = context;
    }

    public async Task<ShowInventory?> GetByShowIdAsync(Guid showId, CancellationToken cancellationToken = default)
    {
        // We MUST include the Seats collection so the aggregate is fully loaded
        // before we attempt to run business logic on it.
        return await _context.ShowInventories
            .Include(x => x.Seats)
            .FirstOrDefaultAsync(x => x.ShowId == showId, cancellationToken);
    }

    public Task UpdateAsync(ShowInventory inventory, CancellationToken cancellationToken = default)
    {
        // EF Core's Change Tracker automatically detects changes to the entity 
        // once it's loaded, but calling Update marks it explicitly.
        _context.ShowInventories.Update(inventory);

        // Note: We don't call SaveChanges here! 
        // The Application layer's Handler dictates when the transaction commits via IUnitOfWork.
        return Task.CompletedTask;
    }
}