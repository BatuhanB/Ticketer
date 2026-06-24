using Ticketer.Domain.Booking.Events;
using Ticketer.Domain.Common;

namespace Ticketer.Domain.Booking.Aggregates;

public class ShowInventory : AggregateRoot
{
    public Guid ShowId { get; private set; }
    public string ShowName { get; private set; }
    // Lock version for concurrency control
    public byte[] Version { get; private set; }

    private readonly List<SeatAvailability> _seats = [];
    public IReadOnlyCollection<SeatAvailability> Seats => _seats.AsReadOnly();

    private ShowInventory() { }

    public ShowInventory(Guid id, Guid showid, string showName, IEnumerable<SeatAvailability> seats) : base(id)
    {
        ShowId = showid;
        ShowName = showName;
        _seats.AddRange(seats);
    }

    /// <summary>
    /// Reserves the requested seats for the user if they are available. Throws exceptions if any of the requested seats are already taken or do not exist in this inventory.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="requestedSeatNumbers"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void ReserveSeats(Guid userId, List<string> requestedSeatNumbers)
    {
        if (requestedSeatNumbers == null || !requestedSeatNumbers.Any())
            throw new ArgumentException("At least one seat must be requested.");

        var seatsToReserve = _seats
            .Where(s => requestedSeatNumbers.Contains(s.SeatNumber))
            .ToList();

        // Rule 1: Ensure all requested seats actually exist for this show
        if (seatsToReserve.Count != requestedSeatNumbers.Count)
            throw new InvalidOperationException("One or more requested seats do not exist in this inventory.");

        // Rule 2: Ensure all requested seats are available
        var unavailableSeats = seatsToReserve.Where(s => !s.IsAvailable).Select(s => s.SeatNumber).ToList();
        if (unavailableSeats.Any())
        {
            // In a real app, you might use a custom DomainException here
            throw new InvalidOperationException($"The following seats are already taken: {string.Join(", ", unavailableSeats)}");
        }

        // Lock the seats
        foreach (var seat in seatsToReserve)
        {
            seat.LockForUser(userId);
        }

        // Raise a domain event
        AddDomainEvent(new SeatsReservedEvent(ShowId, userId, requestedSeatNumbers));
    }
}
