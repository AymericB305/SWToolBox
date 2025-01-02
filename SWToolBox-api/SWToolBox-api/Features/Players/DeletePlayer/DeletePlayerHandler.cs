using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Players.DeletePlayer;

internal sealed class DeletePlayerHandler(SwDbContext context) : IRequestHandler<DeletePlayerCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await context.Players
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (player is null)
        {
            return new NotFound();
        }
        
        context.Players.Remove(player);
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}