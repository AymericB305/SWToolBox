using MediatR;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

public record CreateGuildCommand(string Name) : IRequest<Guild>;

internal sealed class CreateGuildHandler(SwDbContext context) : IRequestHandler<CreateGuildCommand, Guild>
{
    public async Task<Guild> Handle(CreateGuildCommand request, CancellationToken cancellationToken)
    {
        var guild = await context.Guilds.AddAsync(request.ToEntity(), cancellationToken);
        
        return guild.Entity;
    }
}