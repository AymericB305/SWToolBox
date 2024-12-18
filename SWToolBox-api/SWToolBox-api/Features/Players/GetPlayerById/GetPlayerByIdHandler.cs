using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.GetPlayerById;

internal sealed class GetPlayerByIdHandler(SwDbContext context)
    : IRequestHandler<GetPlayerByIdQuery, OneOf<Player, NotFound>>
{
    public async Task<OneOf<Player, NotFound>> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
    {
        var player = await context.Players
            .Include(p => p.GuildPlayers
                .Where(gp => !gp.IsHiddenByGuild && !gp.IsArchivedByPlayer)
                .OrderBy(gp => gp.LeftAt == null)
                    .ThenByDescending(gp => gp.JoinedAt)
                    .ThenByDescending(gp => gp.LeftAt))
                .ThenInclude(gp => gp.Guild)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (player is null)
        {
            return new NotFound();
        }

        return player;
    }
}