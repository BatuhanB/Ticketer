using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Application.Booking.Commands.ReserveSeats;

namespace Ticketer.Presentation.Controllers;

[Route("api/shows")]
[ApiController]
public class BookingController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    public record ReserveSeatsRequest(Guid UserId, List<string> SeatNumbers);

    [HttpPost("{showId}/reserve")]
    public async Task<IActionResult> ReserveSeats(Guid showId, [FromBody] ReserveSeatsRequest request)
    {
        var command = new ReserveSeatsCommand(showId, request.UserId, request.SeatNumbers);

        var result = await _mediator.Send(command);

        return Ok(new { Success = result, Message = "Seats successfully reserved." });
    }
}
