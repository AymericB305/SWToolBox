using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Players.GetPlayerById;

[HttpGet("{id:guid}")]
[Group<PlayersGroup>]
[Authorize(Policy = "WritePlayerData")]
public class GetPlayerByIdEndpoint(ISender sender) : Endpoint<GetPlayerByIdQuery, Results<Ok<GetPlayerByIdResponse>, NotFound>>
{
    public override async Task<Results<Ok<GetPlayerByIdResponse>, NotFound>> ExecuteAsync(GetPlayerByIdQuery req, CancellationToken ct)
    {
        var playerOrNotFound = await sender.Send(req, ct);
        
        return playerOrNotFound.Match<Results<Ok<GetPlayerByIdResponse>, NotFound>>(
            player => TypedResults.Ok(player.ToResponse()),
            notFound => TypedResults.NotFound()
        );
    }
}