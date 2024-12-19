using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.ManageMembers.ChangePlayerRank;

// [HttpPatch("{playerId:guid}/change-rank")]
// [Group<MembersGroup>]
// [Authorize(Policy = "ChangeRank")]
public class ChangePlayerRankEndpoint(ISender sender) : Endpoint<ChangePlayerRankCommand, Results<Ok<ChangePlayerRankResponse>, NotFound>>
{
    public override void Configure()
    {
        Patch("{playerId:guid}/change-rank");
        Group<MembersGroup>();
        Description(b => b
            .Accepts<ChangePlayerRankCommand>("application/json+custom")
            .Produces<Results<Ok<ChangePlayerRankResponse>, NotFound>>(200, "application/json+custom")
            .ProducesProblemFE()
            .ProducesProblemFE<InternalErrorResponse>(500),
        clearDefaults: true);
        Policies("ChangeRank");
    }

    public override async Task<Results<Ok<ChangePlayerRankResponse>, NotFound>> ExecuteAsync(ChangePlayerRankCommand req, CancellationToken ct)
    {
        var playerOrNotFound = await sender.Send(req, ct);

        return playerOrNotFound.Match<Results<Ok<ChangePlayerRankResponse>, NotFound>>(
            player => TypedResults.Ok(player.ToResponse()),
            notFound => TypedResults.NotFound()
        );
    }
}