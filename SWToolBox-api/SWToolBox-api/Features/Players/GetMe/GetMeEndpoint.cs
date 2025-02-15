using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Players.GetMe;

[HttpGet("me")]
public class GetMeEndpoint(ISender sender) : EndpointWithoutRequest<Results<Ok<GetMeResponse>, NotFound>>
{
    public override async Task<Results<Ok<GetMeResponse>, NotFound>> ExecuteAsync(CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userGuid = Guid.Parse(userId ?? string.Empty);
        
        var playerOrNotFound = await sender.Send(new GetMeQuery(userGuid), ct);
        
        return playerOrNotFound.Match<Results<Ok<GetMeResponse>, NotFound>>(
            player => TypedResults.Ok(player.ToResponse()),
            notFound => TypedResults.NotFound()
        );
    }
    
}