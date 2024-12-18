using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Players.ChangePlayerRank;

[HttpPatch("{playerId:guid}/rank")]
[Group<GuildPlayersGroup>]
public class ChangePlayerRankEndpoint(ISender sender) : Endpoint<ChangePlayerRankCommand, Results<Ok<ChangePlayerRankResponse>, NotFound>>
{
    public override async Task<Results<Ok<ChangePlayerRankResponse>, NotFound>> ExecuteAsync(ChangePlayerRankCommand req, CancellationToken ct)
    {
        var playerOrNotFound = await sender.Send(req, ct);

        return playerOrNotFound.Match<Results<Ok<ChangePlayerRankResponse>, NotFound>>(
            player => TypedResults.Ok(player.ToResponse()),
            notFound => TypedResults.NotFound()
        );
    }
}