﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.GetGuildById;

public record GetGuildByIdQuery([FromRoute] Guid GuildId) : IRequest<OneOf<Guild, NotFound>>;
public record GetGuildByIdResponse(
    Guid Id,
    string Name,
    IEnumerable<PlayerResponse> Players,
    IEnumerable<DefenseResponse> Defenses);
public record PlayerResponse(Guid Id, string Name);
public record DefenseResponse(
    Guid Id,
    MonsterResponse MonsterLead,
    MonsterResponse Monster2,
    MonsterResponse Monster3,
    string Description,
    IEnumerable<PlacementResponse> Placements);
public record MonsterResponse(long Id, string Name);
public record PlacementResponse(Guid PlayerId, TowerResponse Tower);
public record TowerResponse(long Id, string Name);

public static class GetGuildByIdMapper
{
    public static GetGuildByIdResponse ToResponse(this Guild guild)
    {
        return new GetGuildByIdResponse(
            guild.Id,
            guild.Name,
            guild.GuildPlayers.Select(gp => gp.Player.ToResponse()),
            guild.Defenses.Select(d => d.ToResponse())
        );
    }

    private static PlayerResponse ToResponse(this Player player)
    {
        return new PlayerResponse(
            player.Id,
            player.Name
        );
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
        return new PlacementResponse(placement.PlayerId, placement.Tower.ToResponse());
    }

    private static TowerResponse ToResponse(this Tower tower)
    {
        return new TowerResponse(tower.Id, tower.Team.Code + tower.Number);
    }
}