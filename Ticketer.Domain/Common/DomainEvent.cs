namespace Ticketer.Domain.Common;

public abstract record class DomainEvent(Guid Id, DateTime OccuredOn)
{
    protected DomainEvent() : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}
