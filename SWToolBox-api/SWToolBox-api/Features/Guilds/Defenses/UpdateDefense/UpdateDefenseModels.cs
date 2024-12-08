using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.Defenses.UpdateDefense;

public record UpdateDefenseCommand([FromRoute] Guid GuildId, [FromRoute] Guid Id, long MonsterLeadId, long Monster2Id, long Monster3Id, string Description)
    : IRequest<OneOf<GuildDefense, NotFound>>;
public record UpdateDefenseResponse(DefenseResponse Defense, string Description);
public record DefenseResponse(MonsterResponse MonsterLead, MonsterResponse Monster2, MonsterResponse Monster3);
public record MonsterResponse(long Id, string Name);

public static class UpdateDefenseMapper
{
    public static UpdateDefenseResponse ToResponse(this GuildDefense guildDefense)
    {
        return new UpdateDefenseResponse(guildDefense.Defense.ToResponse(), guildDefense.Description);
    }
    
    private static DefenseResponse ToResponse(this Defense defense)
    {
        return new DefenseResponse(
            defense.MonsterLead.ToResponse(),
            defense.Monster2.ToResponse(),
            defense.Monster3.ToResponse()
        );
    }

    private static MonsterResponse ToResponse(this Monster monster)
    {
        return new MonsterResponse(monster.Id, monster.Name);
    }
}