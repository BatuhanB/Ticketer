using Ticketer.Domain.Booking.Aggregates;

namespace Ticketer.Domain.Booking;

public interface IShowInventoryRepository
{
    Task<ShowInventory?> GetByShowIdAsync(Guid showId, CancellationToken cancellationToken = default);
    Task UpdateAsync(ShowInventory inventory, CancellationToken cancellationToken = default);
}
