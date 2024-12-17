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
        var existingPlayer = await context.Players
            .Include(p => p.GuildPlayers)
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
        
        if (existingPlayer is not null)
        {
            if (existingPlayer.GuildPlayers.Any(gp => gp.GuildId == request.GuildId))
            {
                return new Existing();
            }
            
            await AddGuildPlayer(request, existingPlayer, cancellationToken);

            return existingPlayer;
        }
        
        var player = await context.Players.AddAsync(request.ToEntity(), cancellationToken);
        await AddGuildPlayer(request, player.Entity, cancellationToken);
        
        return player.Entity;
    }

    private async Task AddGuildPlayer(CreatePlayerCommand request,
        Player existingPlayer, CancellationToken cancellationToken)
    {
        var guildPlayer = new GuildPlayer
        {
            GuildId = request.GuildId,
            Player = existingPlayer,
            RankId = 1L, // member
        };
        
        await context.GuildPlayers.AddAsync(guildPlayer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}