using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManagePlacements.UpdatePlacement;

public record UpdatePlacementCommand(
    [FromRoute] Guid GuildId,
    [FromRoute] Guid Id,
    long TowerId,
    Guid PlayerId,
    Guid DefenseId) : IRequest<OneOf<Placement, Failure, NotFound>>;
public record UpdatePlacementResponse(PlayerResponse Player, DefenseResponse Defense, TowerResponse Tower);
public record PlayerResponse(Guid Id, string Name);
public record TowerResponse(long Id, string Name);
public record DefenseResponse(
    Guid Id,
    MonsterResponse MonsterLead,
    MonsterResponse Monster2,
    MonsterResponse Monster3,
    string Description);
public record MonsterResponse(long Id, string Name);

public static class CreatePlacementMapper
{
    public static UpdatePlacementResponse ToResponse(this Placement placement)
    {
        return new UpdatePlacementResponse(placement.Player.ToResponse(), placement.Defense.ToResponse(), placement.Tower.ToResponse());
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
            defense.Description
        );
    }

    private static MonsterResponse ToResponse(this Monster monster)
    {
        return new MonsterResponse(monster.Id, monster.Name);
    }

    private static TowerResponse ToResponse(this Tower tower)
    {
        return new TowerResponse(tower.Id, tower.Team.Code + tower.Number);
    }
}