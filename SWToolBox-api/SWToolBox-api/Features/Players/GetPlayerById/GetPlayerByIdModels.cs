using System.Collections;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.GetPlayerById;

public record GetPlayerByIdQuery([FromRoute] Guid Id) : IRequest<OneOf<Player, NotFound>>;

public record GetPlayerByIdResponse(Guid Id, string Name, IEnumerable<GuildResponse> Guilds);
public record GuildResponse(Guid Id, string Name, IEnumerable<DefenseResponse> Defenses, DateTime JoinedAt, DateTime? LeftAt, RankResponse Rank);
public record DefenseResponse(
    Guid Id,
    MonsterResponse MonsterLead,
    MonsterResponse Monster2,
    MonsterResponse Monster3,
    string Description,
    IEnumerable<PlacementResponse> Placements);
public record MonsterResponse(long Id, string Name);
public record PlacementResponse(Guid PlayerId, TowerResponse Tower, short Wins, short Losses);
public record TowerResponse(long Id, string Name);
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
            guild.Defenses.Select(d => d.ToResponse()),
            joinedAt,
            leftAt,
            rank.ToResponse()
        );
    }

    private static RankResponse ToResponse(this Rank rank)
    {
        return new RankResponse(rank.Id, rank.Name);
    }

    private static DefenseResponse ToResponse(this Defense defense)
    {
        return new DefenseResponse(
            defense.Id,
            defense.MonsterLead.ToResponse(),
            defense.Monster2.ToResponse(),
            defense.Monster3.ToResponse(),
            defense.Description,
            defense.Placements.Select(pdt => pdt.ToResponse())
        );
    }

    private static MonsterResponse ToResponse(this Monster monster)
    {
        return new MonsterResponse(monster.Id, monster.Name);
    }

    private static PlacementResponse ToResponse(this Placement placement)
    {
        return new PlacementResponse(placement.PlayerId, placement.Tower.ToResponse(), placement.Wins, placement.Losses);
    }

    private static TowerResponse ToResponse(this Tower tower)
    {
        return new TowerResponse(tower.Id, tower.Team.Code + tower.Number);
    }
}