﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;
using NotFound = OneOf.Types.NotFound;

namespace SWToolBox_api.Features.Guilds.Admin.UpdateGuild;

internal sealed class UpdateGuildHandler(SwDbContext context) : IRequestHandler<UpdateGuildCommand, OneOf<Guild, NotFound, Existing>>
{
    public async Task<OneOf<Guild, NotFound, Existing>> Handle(UpdateGuildCommand request, CancellationToken cancellationToken)
    {
        var guild = await context.Guilds
            .FirstOrDefaultAsync(g => g.Id == request.GuildId, cancellationToken);

        if (guild is null)
        {
            return new NotFound();
        }
        
        var existingGuildName = await context.Guilds
            .FirstOrDefaultAsync(g => g.Name == guild.Name, cancellationToken);

        if (existingGuildName is not null)
        {
            return new Existing();
        }
        
        guild.Name = request.Name;
        await context.SaveChangesAsync(cancellationToken);
        
        return guild;
    }
}