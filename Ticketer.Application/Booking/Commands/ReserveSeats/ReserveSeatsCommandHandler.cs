using MediatR;
using Ticketer.Application.Common.Interfaces;
using Ticketer.Domain.Booking;

namespace Ticketer.Application.Booking.Commands.ReserveSeats;

public class ReserveSeatsCommandHandler(
    IUnitOfWork unitOfWork,
    IShowInventoryRepository showInventoryRepository) : IRequestHandler<ReserveSeatsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IShowInventoryRepository _showInventoryRepository = showInventoryRepository;

    public async Task<bool> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
    {
        //Fetch the show inventory for the given show ID
        var showInventory = await _showInventoryRepository.GetByShowIdAsync(request.ShowId);

        if (showInventory == null)
            throw new ArgumentNullException($"Given {request.ShowId} was not found!");

        showInventory.ReserveSeats(request.UserId, request.SeatNumbers);
        await _showInventoryRepository.UpdateAsync(showInventory, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
