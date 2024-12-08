using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.Defenses.UpdateDefense;

public class UpdateDefenseHandler(SwDbContext context) : IRequestHandler<UpdateDefenseCommand, OneOf<GuildDefense, NotFound>>
{
    public async Task<OneOf<GuildDefense, NotFound>> Handle(UpdateDefenseCommand request,
        CancellationToken cancellationToken)
    {
        var existingGuildDefense = await context.GuildDefenses
            .Include(gd => gd.Defense)
            .FirstOrDefaultAsync(gd => gd.GuildId == request.GuildId && gd.DefenseId == request.Id, cancellationToken);

        if (existingGuildDefense is null)
        {
            return new NotFound();
        }

        if (IsTheSameDefense(request, existingGuildDefense))
        {
            await UpdateGuildDefense(request, existingGuildDefense, cancellationToken);

            return existingGuildDefense;
        }

        var newGuildDefense = await ReplaceGuildDefense(request, existingGuildDefense, cancellationToken);

        return newGuildDefense.Entity;
    }

    private async Task UpdateGuildDefense(UpdateDefenseCommand request, GuildDefense existingGuildDefense, CancellationToken cancellationToken)
    {
        existingGuildDefense.Description = request.Description;
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<EntityEntry<GuildDefense>> ReplaceGuildDefense(UpdateDefenseCommand request, GuildDefense existingGuildDefense, CancellationToken cancellationToken)
    {
        var defense = await GetDefense(request, cancellationToken);

        var guildDefense = await context.GuildDefenses.AddAsync(new GuildDefense
        {
            GuildId = request.GuildId,
            Defense = defense,
            Description = request.Description,
        }, cancellationToken);

        context.GuildDefenses.Remove(existingGuildDefense);
        await context.SaveChangesAsync(cancellationToken);
        return guildDefense;
    }

    private static bool IsTheSameDefense(UpdateDefenseCommand request, GuildDefense existingGuildDefense)
    {
        return existingGuildDefense.Defense.MonsterLeadId == request.MonsterLeadId
               && existingGuildDefense.Defense.Monster2Id == request.Monster2Id
               && existingGuildDefense.Defense.Monster3Id == request.Monster3Id;
    }

    private async Task<Defense> GetDefense(UpdateDefenseCommand request, CancellationToken cancellationToken)
    {
        return await context.Defenses
            .FirstOrDefaultAsync(d => d.MonsterLeadId == request.MonsterLeadId
                && d.Monster2Id == request.Monster2Id
                && d.Monster3Id == request.Monster3Id, cancellationToken)
        ?? new Defense
        {
            MonsterLeadId = request.MonsterLeadId,
            Monster2Id = request.Monster2Id,
            Monster3Id = request.Monster3Id,
        };
    }
}