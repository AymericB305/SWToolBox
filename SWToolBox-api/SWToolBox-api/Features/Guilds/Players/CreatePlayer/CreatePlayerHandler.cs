using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.Players.CreatePlayer;

internal sealed class CreatePlayerHandler(SwDbContext context) : IRequestHandler<CreatePlayerCommand, OneOf<Player, Existing>>
{
    public async Task<OneOf<Player, Existing>> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var existingPlayer = await context
            .Players
            .Include(p => p.GuildPlayers)
                .ThenInclude(gp => gp.Guild)
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
        
        if (existingPlayer is not null)
        {
            return new Existing();
        }
        
        var player = await context.Players.AddAsync(request.ToEntity(), cancellationToken);
        var guildPlayer = new GuildPlayer
        {
            GuildId = request.GuildId,
            Player = player.Entity,
        };
        await context.GuildPlayers.AddAsync(guildPlayer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return player.Entity;
    }
}