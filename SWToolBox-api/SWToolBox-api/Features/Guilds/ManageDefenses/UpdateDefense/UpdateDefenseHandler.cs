using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManageDefenses.UpdateDefense;

public class UpdateDefenseHandler(SwDbContext context)
    : IRequestHandler<UpdateDefenseCommand, OneOf<Defense, NotFound, Existing>>
{
    public async Task<OneOf<Defense, NotFound, Existing>> Handle(UpdateDefenseCommand request,
        CancellationToken cancellationToken)
    {
        var defense = await context.Defenses
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (defense is null)
        {
            return new NotFound();
        }

        var existingDefense = await context.Defenses
            .FirstOrDefaultAsync(
                d => d.Id != request.Id
                     && d.GuildId == request.GuildId
                     && d.MonsterLeadId == request.MonsterLeadId
                     && d.Monster2Id == request.Monster2Id
                     && d.Monster3Id == request.Monster3Id,
                cancellationToken);

        if (existingDefense is not null)
        {
            return new Existing();
        }

        defense.MonsterLeadId = request.MonsterLeadId;
        defense.Monster2Id = request.Monster2Id;
        defense.Monster3Id = request.Monster3Id;
        defense.Description = request.Description;

        await context.SaveChangesAsync(cancellationToken);

        return defense;
    }
}