﻿using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManageMembers.ChangePlayerRank;

public record ChangePlayerRankCommand(
    [FromRoute] Guid GuildId,
    [FromRoute] Guid PlayerId) : IRequest<OneOf<GuildPlayer, NotFound>>
{
    [QueryParam]
    public int RankId { get; init; }
}

public record ChangePlayerRankResponse(Guid PlayerId, RankResponse Rank);

public record RankResponse(long Id, string Name);

public static class ChangePlayerRankMapper
{
    public static ChangePlayerRankResponse ToResponse(this GuildPlayer guildPlayer)
    {
        return new ChangePlayerRankResponse(guildPlayer.PlayerId, guildPlayer.Rank.ToResponse());
    }

    private static RankResponse ToResponse(this Rank rank)
    {
        return new RankResponse(rank.Id, rank.Name);
    }
}