using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.UpdatePlayer;

public class UpdatePlayerHandler(SwDbContext context) : IRequestHandler<UpdatePlayerCommand, OneOf<Player, NotFound>>
{
    public async Task<OneOf<Player, NotFound>> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await context.Players
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
        if (player is null)
        {
            return new NotFound();
        }
        
        player.Name = request.Name;
        await context.SaveChangesAsync(cancellationToken);
        
        return player;
    }
}