using MediatR;

namespace Ticketer.Application.Booking.Commands.ReserveSeats;

// A Record representing the input required to perform the action.
// IRequest<bool> indicates that this command will return a boolean result.
public record ReserveSeatsCommand(
    Guid ShowId,
    Guid UserId,
    List<string> SeatNumbers) : IRequest<bool>;