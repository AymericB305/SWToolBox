using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.Players.RemovePlayerFromGuild;

public record RemovePlayerFromGuildCommand(Guid GuildId, Guid Id) : IRequest<string>;

internal sealed class RemovePlayerFromGuildHandler(SwDbContext context) : IRequestHandler<RemovePlayerFromGuildCommand, string>
{
    public async Task<string> Handle(RemovePlayerFromGuildCommand request, CancellationToken cancellationToken)
    {
        var guildPlayer = await context.GuildPlayers
            .FirstOrDefaultAsync(gp => gp.PlayerId == request.Id && gp.GuildId == request.GuildId, cancellationToken);

        if (guildPlayer == null)
        {
            return "Player wasn't part of your guild.";
        }

        if (guildPlayer.LeftAt != null)
        {
            return "Player already has left the guild.";
        }
        
        guildPlayer.LeftAt = DateTime.Now;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return "Player was removed from the guild.";
    }
}