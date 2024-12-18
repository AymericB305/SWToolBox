using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.Defenses.CreateDefense;

internal sealed class CreateDefenseHandler(SwDbContext context) : IRequestHandler<CreateDefenseCommand, OneOf<Defense, NotFound, Existing>>
{
    public async Task<OneOf<Defense, NotFound, Existing>> Handle(CreateDefenseCommand request, CancellationToken cancellationToken)
    {
        var guild = await context.Guilds.FirstOrDefaultAsync(g => g.Id == request.GuildId, cancellationToken);

        if (guild is null)
        {
            return new NotFound();
        }
        
        var existingDefense = await context.Defenses
            .FirstOrDefaultAsync(d => d.GuildId == request.GuildId
                && d.MonsterLeadId == request.MonsterLeadId
                && d.Monster2Id == request.Monster2Id
                && d.Monster3Id == request.Monster3Id, cancellationToken);

        if (existingDefense is not null)
        {
            return new Existing();
        }

        var defense = await context.Defenses.AddAsync(new Defense
        {
            MonsterLeadId = request.MonsterLeadId,
            Monster2Id = request.Monster2Id,
            Monster3Id = request.Monster3Id,
            Description = request.Description,
        }, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
            
        return defense.Entity;
    }
}