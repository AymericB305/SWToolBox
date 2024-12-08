﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.Defenses.DeleteDefense;

internal sealed class DeleteDefenseHandler(SwDbContext context) : IRequestHandler<DeleteDefenseCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(DeleteDefenseCommand request, CancellationToken cancellationToken)
    {
        var defense = await context.GuildDefenses
            .FirstOrDefaultAsync(gd => gd.DefenseId == request.Id && gd.GuildId == request.GuildId, cancellationToken);

        if (defense is null)
        {
            return new NotFound();
        }
        
        context.GuildDefenses.Remove(defense);
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}