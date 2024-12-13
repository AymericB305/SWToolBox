using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.Defenses.UpdateDefense;

public class UpdateDefenseHandler(SwDbContext context) : IRequestHandler<UpdateDefenseCommand, OneOf<Defense, NotFound>>
{
    public async Task<OneOf<Defense, NotFound>> Handle(UpdateDefenseCommand request,
        CancellationToken cancellationToken)
    {
        var existingGuildDefense = await context.Defenses
            .FirstOrDefaultAsync(d => d.GuildId == request.GuildId && d.Id == request.Id, cancellationToken);

        if (existingGuildDefense is null)
        {
            return new NotFound();
        }
        
        existingGuildDefense.MonsterLeadId = request.MonsterLeadId;
        existingGuildDefense.Monster2Id = request.Monster2Id;
        existingGuildDefense.Monster3Id = request.Monster3Id;
        existingGuildDefense.Description = request.Description;
        
        await context.SaveChangesAsync(cancellationToken);

        return existingGuildDefense;
    }
}