using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.GetPlayerById;

public record GetPlayerByIdQuery([FromRoute] Guid Id) : IRequest<OneOf<Player, NotFound>>;

public record GetPlayerByIdResponse(Guid Id, string Name, IEnumerable<GuildResponse> Guilds);
public record GuildResponse(Guid Id, string Name, DateTime JoinedAt, DateTime? LeftAt, RankResponse Rank);
public record RankResponse(long Id, string Name);

public static class GetGuildByIdMapper
{
    public static GetPlayerByIdResponse ToResponse(this Player player)
    {
        return new GetPlayerByIdResponse(
            player.Id,
            player.Name,
            player.GuildPlayers.Select(gp => gp.Guild.ToResponse(gp.JoinedAt, gp.LeftAt, gp.Rank))
        );
    }

    private static GuildResponse ToResponse(this Guild guild, DateTime joinedAt, DateTime? leftAt, Rank rank)
    {
        return new GuildResponse(
            guild.Id,
            guild.Name,
            joinedAt,
            leftAt,
            rank.ToResponse()
        );
    }

    private static RankResponse ToResponse(this Rank rank)
    {
        return new RankResponse(rank.Id, rank.Name);
    }
}