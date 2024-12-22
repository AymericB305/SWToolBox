using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManageMembers.AddMember;

internal sealed class AddMemberHandler(SwDbContext context) : IRequestHandler<AddMemberCommand, OneOf<Player, Existing, NotFound>>
{
    public async Task<OneOf<Player, Existing, NotFound>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        if (request.PlayerId is not null)
        {
            var player = await context.Players
                .FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);

            if (player is null)
            {
                return new NotFound();
            }
            
            await AddGuildPlayerAndSave(request.GuildId, player, cancellationToken);
        
            return player;
        }
        
        var existingPlayer = await context.Players
            .Include(p => p.GuildPlayers)
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
        
        if (existingPlayer is not null)
        {
            if (existingPlayer.GuildPlayers.Any(gp => gp.GuildId == request.GuildId))
            {
                return new Existing();
            }
            
            await AddGuildPlayerAndSave(request.GuildId, existingPlayer, cancellationToken);

            return existingPlayer;
        }
        
        var newPlayer = await context.Players
            .AddAsync(request.ToEntity(), cancellationToken);
        
        await AddGuildPlayerAndSave(request.GuildId, newPlayer.Entity, cancellationToken);
        
        return newPlayer.Entity;
    }

    private async Task AddGuildPlayerAndSave(Guid guildId, Player existingPlayer, CancellationToken cancellationToken)
    {
        var guildPlayer = new GuildPlayer
        {
            GuildId = guildId,
            Player = existingPlayer,
            RankId = 1L, // member
        };
        
        await context.GuildPlayers.AddAsync(guildPlayer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}