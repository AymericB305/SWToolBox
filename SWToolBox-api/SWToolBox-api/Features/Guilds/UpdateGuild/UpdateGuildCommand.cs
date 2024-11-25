using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.UpdateGuild;

public record UpdateGuildCommand(Guid Id, string Name) : IRequest<Guild?>;

internal sealed class UpdateGuildHandler(SwDbContext context) : IRequestHandler<UpdateGuildCommand, Guild?>
{
    public async Task<Guild?> Handle(UpdateGuildCommand request, CancellationToken cancellationToken)
    {
        var existingGuild = await context.Guilds.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (existingGuild is null)
        {
            return null;
        }
        
        existingGuild.Name = request.Name;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return existingGuild;
    }
}