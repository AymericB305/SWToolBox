using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Players.UpdatePlayer;

[HttpPut("{id:guid}")]
[Group<PlayersGroup>]
[Authorize(Policy = "WritePlayerData")]
public class UpdatePlayerEndpoint(ISender sender) : Endpoint<UpdatePlayerCommand, Results<Ok<UpdatePlayerResponse>, NotFound>>
{
    public override async Task<Results<Ok<UpdatePlayerResponse>, NotFound>> ExecuteAsync(UpdatePlayerCommand req, CancellationToken ct)
    {
        var playerOrNotFound = await sender.Send(req, ct);

        return playerOrNotFound.Match<Results<Ok<UpdatePlayerResponse>, NotFound>>(
            player => TypedResults.Ok(player.ToResponse()),
            notFound => TypedResults.NotFound()
        );
    }
}