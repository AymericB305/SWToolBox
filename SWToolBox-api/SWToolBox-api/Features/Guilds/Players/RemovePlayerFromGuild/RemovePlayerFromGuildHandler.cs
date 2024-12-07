using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using NotFound = OneOf.Types.NotFound;

namespace SWToolBox_api.Features.Guilds.Players.RemovePlayerFromGuild;

internal sealed class RemovePlayerFromGuildHandler(SwDbContext context) : IRequestHandler<RemovePlayerFromGuildCommand, OneOf<Success, Failure, NotFound>>
{
    public async Task<OneOf<Success, Failure, NotFound>> Handle(RemovePlayerFromGuildCommand request, CancellationToken cancellationToken)
    {
        var guildPlayer = await context.GuildPlayers
            .FirstOrDefaultAsync(gp => gp.PlayerId == request.Id && gp.GuildId == request.GuildId, cancellationToken);

        if (guildPlayer is null)
        {
            return new NotFound();
        }

        if (guildPlayer.LeftAt is not null)
        {
            return new Failure("Player has already left the guild.");
        }
        
        guildPlayer.LeftAt = DateTime.Now;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}