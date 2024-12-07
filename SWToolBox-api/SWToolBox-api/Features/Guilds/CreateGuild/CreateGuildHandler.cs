using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

internal sealed class CreateGuildHandler(SwDbContext context) : IRequestHandler<CreateGuildCommand, OneOf<Guild, Existing>>
{
    public async Task<OneOf<Guild, Existing>> Handle(CreateGuildCommand request, CancellationToken cancellationToken)
    {
        var existingGuild = await context.Guilds.FirstOrDefaultAsync(g => g.Name == request.Name, cancellationToken);

        if (existingGuild is not null)
        {
            return new Existing();
        }
        
        var guild = await context.Guilds.AddAsync(request.ToEntity(), cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return guild.Entity;
    }
}