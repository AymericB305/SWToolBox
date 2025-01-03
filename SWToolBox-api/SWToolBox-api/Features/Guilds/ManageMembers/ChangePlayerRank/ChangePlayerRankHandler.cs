﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManageMembers.ChangePlayerRank;

public class ChangePlayerRankHandler(SwDbContext context) : IRequestHandler<ChangePlayerRankCommand, OneOf<GuildPlayer, NotFound>>
{
    public async Task<OneOf<GuildPlayer, NotFound>> Handle(ChangePlayerRankCommand request, CancellationToken cancellationToken)
    {
        var guildPlayer = await context.GuildPlayers
            .FirstOrDefaultAsync(gp => gp.GuildId == request.GuildId && gp.PlayerId == request.PlayerId, cancellationToken);

        if (guildPlayer is null)
        {
            return new NotFound();
        }
        
        var rank = await context.Ranks
            .FirstOrDefaultAsync(r => r.Id == request.RankId, cancellationToken);
        
        guildPlayer.Rank = rank!;
        guildPlayer.RankId = request.RankId;
        await context.SaveChangesAsync(cancellationToken);

        return guildPlayer;
    }
}