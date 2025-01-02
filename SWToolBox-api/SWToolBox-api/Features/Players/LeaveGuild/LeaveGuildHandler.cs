using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Players.LeaveGuild;

internal sealed class LeaveGuildHandler(SwDbContext context) : IRequestHandler<LeaveGuildCommand, OneOf<Success, Failure, NotFound>>
{
    public async Task<OneOf<Success, Failure, NotFound>> Handle(LeaveGuildCommand request, CancellationToken cancellationToken)
    {
        var guildPlayer = await context.GuildPlayers
            .FirstOrDefaultAsync(gp => gp.PlayerId == request.Id && gp.GuildId == request.GuildId, cancellationToken);

        if (guildPlayer is null)
        {
            return new NotFound();
        }

        if (guildPlayer.LeftAt is not null)
        {
            return new Failure("You already left this guild.");
        }
        
        guildPlayer.LeftAt = DateTime.Now;
        guildPlayer.IsArchivedByPlayer = request.ArchiveGuild;
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}