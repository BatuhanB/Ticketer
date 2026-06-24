namespace Ticketer.Domain.Common;

public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot(Guid id): base(id){}
    protected AggregateRoot() { }

    /// <summary>
    /// Adds a domain event to the aggregate root's list of domain events.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Deletes a domain event from the aggregate root's list of domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
