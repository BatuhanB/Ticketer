using FluentValidation;

namespace Ticketer.Application.Booking.Commands.ReserveSeats;

// This validates the input *before* it hits the CommandHandler.
// We prevent bad data from reaching the Domain entirely.
public class ReserveSeatsCommandValidator : AbstractValidator<ReserveSeatsCommand>
{
    public ReserveSeatsCommandValidator()
    {
        RuleFor(v => v.ShowId)
            .NotEmpty().WithMessage("ShowId is required.");

        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(v => v.SeatNumbers)
            .NotNull()
            .NotEmpty().WithMessage("At least one seat must be requested.")
            .Must(seats => seats.Count <= 6).WithMessage("You cannot reserve more than 6 seats at once.");
    }
}