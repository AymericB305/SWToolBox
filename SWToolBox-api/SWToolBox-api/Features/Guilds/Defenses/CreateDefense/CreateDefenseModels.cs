using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;
using NotFound = SWToolBox_api.Common.Models.NotFound;

namespace SWToolBox_api.Features.Guilds.Defenses.CreateDefense;

public record CreateDefenseCommand([FromRoute] Guid GuildId, long MonsterLeadId, long Monster2Id, long Monster3Id) : IRequest<OneOf<Defense, NotFound, Existing>>;
public record CreateDefenseResponse(Guid Id, MonsterResponse MonsterLead, MonsterResponse Monster2, MonsterResponse Monster3);
public record MonsterResponse(long Id, string Name);

public static class CreateDefenseMapper
{
    public static CreateDefenseResponse ToResponse(this Defense defense)
    {
        return new CreateDefenseResponse(defense.Uid, defense.MonsterLead.ToResponse(), defense.Monster2.ToResponse(), defense.Monster3.ToResponse());
    }

    private static MonsterResponse ToResponse(this Monster monster)
    {
        return new MonsterResponse(monster.Id, monster.Name);
    }
}