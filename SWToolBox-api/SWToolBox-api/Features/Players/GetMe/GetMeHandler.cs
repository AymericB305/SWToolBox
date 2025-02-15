using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.GetMe;

internal sealed class GetMeHandler(SwDbContext context)
    : IRequestHandler<GetMeQuery, OneOf<Player, NotFound>>
{
    public async Task<OneOf<Player, NotFound>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var player = await context.Players
            .Include(p => p.GuildPlayers
                .Where(gp => !gp.IsHiddenByGuild && !gp.IsArchivedByPlayer)
                .OrderByDescending(gp => gp.LeftAt == null)
                .ThenByDescending(gp => gp.JoinedAt)
                .ThenByDescending(gp => gp.LeftAt))
            .ThenInclude(gp => gp.Guild)
            .Include(p => p.GuildPlayers)
            .ThenInclude(gp => gp.Rank)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (player is null)
        {
            return new NotFound();
        }

        return player;
    }
}