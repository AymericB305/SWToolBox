using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.ManagePlacements.DeletePlacement;

public class DeletePlacementHandler(SwDbContext context) : IRequestHandler<DeletePlacementCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(DeletePlacementCommand request, CancellationToken cancellationToken)
    {
        var placement = await context.Placements
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (placement is null)
        {
            return new NotFound();
        }
        
        context.Placements.Remove(placement);
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}