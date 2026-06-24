using Ticketer.Domain.Common;

namespace Ticketer.Domain.Booking.Aggregates;

public class SeatAvailability : Entity
{
    public string SeatNumber { get; private set; }
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; }
    public Guid? ReservedByUserId { get; private set; }

    public SeatAvailability(Guid id, string seatNumber, decimal price) : base(id)
    {
        SeatNumber = seatNumber;
        Price = price;
        IsAvailable = true;
    }

    /// <summary>
    /// Reserves the locked seat for user if it already reserved it throws an exception
    /// </summary>
    /// <param name="userId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    internal void LockForUser(Guid userId)
    {
        if (!IsAvailable)
        {
            throw new InvalidOperationException($"Seat {SeatNumber} is already locked.");
        }
        IsAvailable = false;
        ReservedByUserId = userId;
    }
}