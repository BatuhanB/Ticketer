namespace Ticketer.Application.Common.Interfaces;

// The Unit of Work ensures that all changes (and domain events) 
// are committed to the database in a single transaction.
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
