using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.Players.CreatePlayer;

public record CreatePlayerCommand(string Name, Guid GuildId) : IRequest<CreatePlayerDto>;

internal sealed class CreatePlayerHandler(SwDbContext context) : IRequestHandler<CreatePlayerCommand, CreatePlayerDto>
{
    public async Task<CreatePlayerDto> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var existingPlayer = await context
            .Players
            .Include(p => p.GuildPlayers)
                .ThenInclude(gp => gp.Guild)
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
        
        if (existingPlayer is not null)
        {
            return existingPlayer.ToDto(false, $"A Player with the name {request.Name} already exists.");
        }
        
        var player = await context.Players.AddAsync(request.ToEntity(), cancellationToken);
        var guildPlayer = new GuildPlayer
        {
            GuildId = request.GuildId,
            Player = player.Entity,
        };
        await context.GuildPlayers.AddAsync(guildPlayer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return player.Entity.ToDto(true);
    }
}