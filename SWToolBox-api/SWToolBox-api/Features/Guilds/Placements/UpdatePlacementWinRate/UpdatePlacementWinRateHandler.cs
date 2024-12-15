using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.Placements.UpdatePlacementWinRate;

public class UpdatePlacementWinRateHandler(SwDbContext context) : IRequestHandler<UpdatePlacementWinRateCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(UpdatePlacementWinRateCommand request, CancellationToken cancellationToken)
    {
        var placement = await context.Placements
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (placement is null)
        {
            return new NotFound();
        }
        
        placement.Wins = request.Wins;
        placement.Losses = request.Losses;
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}