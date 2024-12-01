using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

public record CreateGuildCommand(string Name) : IRequest<CreateGuildDto>;

internal sealed class CreateGuildHandler(SwDbContext context) : IRequestHandler<CreateGuildCommand, CreateGuildDto>
{
    public async Task<CreateGuildDto> Handle(CreateGuildCommand request, CancellationToken cancellationToken)
    {
        var existingGuild = await context.Guilds.FirstOrDefaultAsync(g => g.Name == request.Name, cancellationToken);

        if (existingGuild is not null)
        {
            return existingGuild.ToDto(false, $"A Guild with the name {request.Name} already exists.");
        }
        
        var guild = await context.Guilds.AddAsync(request.ToEntity(), cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return guild.Entity.ToDto(true);
    }
}