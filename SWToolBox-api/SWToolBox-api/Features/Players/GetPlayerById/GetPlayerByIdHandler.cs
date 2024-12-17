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
            .Where(p => p.Id == request.Id)
            .Select(p => SelectPlayer(p, request.Id))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null)
        {
            return new NotFound();
        }

        return player;
    }

    private static Player SelectPlayer(Player p, Guid requestId)
    {
        return new Player
        {
            Id = p.Id,
            Name = p.Name,
            GuildPlayers = p.GuildPlayers
                .Where(gp => !gp.IsHiddenByGuild && !gp.IsArchivedByPlayer)
                .Select(gp => SelectGuildPlayer(gp, requestId)).ToList()
        };
    }

    private static GuildPlayer SelectGuildPlayer(GuildPlayer gp, Guid requestId)
    {
        return new GuildPlayer
        {
            JoinedAt = gp.JoinedAt,
            LeftAt = gp.LeftAt,
            Rank = gp.Rank,
            Guild = SelectGuild(gp, requestId)
        };
    }

    private static Guild SelectGuild(GuildPlayer gp, Guid requestId)
    {
        return new Guild
        {
            Id = gp.Guild.Id,
            Name = gp.Guild.Name,
            Defenses = gp.Guild.Defenses
                .Where(d => d.Placements.Any(plt => plt.PlayerId == requestId))
                .Select(SelectDefense).ToList()
        };
    }

    private static Defense SelectDefense(Defense d)
    {
        return new Defense
        {
            Id = d.Id,
            MonsterLead = d.MonsterLead,
            Monster2 = d.Monster2,
            Monster3 = d.Monster3,
            Placements = d.Placements.Select(SelectPlacement).ToList()
        };
    }

    private static Placement SelectPlacement(Placement plt)
    {
        return new Placement
        {
            Id = plt.Id,
            PlayerId = plt.PlayerId,
            Tower = new Tower
            {
                Id = plt.Tower.Id,
                Number = plt.Tower.Number,
                Team = plt.Tower.Team
            }
        };
    }
}