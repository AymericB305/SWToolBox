using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Guilds.ManageMembers.HideDataFromPlayer;

public class HideDataFromPlayerHandler(SwDbContext context) : IRequestHandler<HideDataFromPlayerCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(HideDataFromPlayerCommand request, CancellationToken cancellationToken)
    {
        var guildPlayer = await context.GuildPlayers
            .FirstOrDefaultAsync(gp => gp.GuildId == request.GuildId && gp.PlayerId == request.Id, cancellationToken);

        if (guildPlayer is null)
        {
            return new NotFound();
        }
        
        guildPlayer.IsHiddenByGuild = true;
        await context.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}