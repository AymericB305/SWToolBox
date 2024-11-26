using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.UpdateGuild;

public record UpdateGuildCommand(Guid Id, string Name) : IRequest<UpdateGuildDto?>;

internal sealed class UpdateGuildHandler(SwDbContext context) : IRequestHandler<UpdateGuildCommand, UpdateGuildDto?>
{
    public async Task<UpdateGuildDto?> Handle(UpdateGuildCommand request, CancellationToken cancellationToken)
    {
        var existingGuild = await context.Guilds.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (existingGuild is null)
        {
            return null;
        }
        
        var existingGuildName = await context.Guilds.FirstOrDefaultAsync(g => g.Name == existingGuild.Name, cancellationToken);

        if (existingGuildName is not null)
        {
            return existingGuildName.ToDto(false);
        }
        
        existingGuild.Name = request.Name;
        await context.SaveChangesAsync(cancellationToken);
        
        return existingGuild.ToDto(true);
    }
}