using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Players.ArchiveGuild;

internal sealed class ArchiveGuildHandler(SwDbContext context) : IRequestHandler<ArchiveGuildCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(ArchiveGuildCommand request, CancellationToken cancellationToken)
    {
        var guildPlayer = await context.GuildPlayers
            .FirstOrDefaultAsync(gp => gp.GuildId == request.GuildId && gp.PlayerId == request.Id, cancellationToken);

        if (guildPlayer is null)
        {
            return new NotFound();
        }
        
        guildPlayer.IsArchivedByPlayer = true;
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}