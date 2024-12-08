using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
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
            return new NotFound("Guild not found.");
        }

        var defense = await context.Defenses
            .FirstOrDefaultAsync(d => d.MonsterLeadId == request.MonsterLeadId
                && d.Monster2Id == request.Monster2Id
                && d.Monster3Id == request.Monster3Id, cancellationToken)
        ?? new Defense
        {
            MonsterLeadId = request.MonsterLeadId,
            Monster2Id = request.Monster2Id,
            Monster3Id = request.Monster3Id,
        };
        
        var existingGuildDefense = await context.GuildDefenses
            .FirstOrDefaultAsync(gd => gd.GuildId == request.GuildId
                && gd.DefenseId == defense.Id, cancellationToken);

        if (existingGuildDefense is not null)
        {
            return new Existing();
        }

        var guildDefense = await context.GuildDefenses.AddAsync(new GuildDefense
        {
            GuildId = request.GuildId,
            Defense = defense,
            Description = request.Description,
        }, cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
            
        return guildDefense.Entity.Defense;
    }
}