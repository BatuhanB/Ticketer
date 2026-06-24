using Ticketer.Domain.Common;

namespace Ticketer.Domain.Booking.Events;

public record SeatsReservedEvent(Guid ShowId, Guid UserId, IEnumerable<string> SeatNumbers) : DomainEvent;
