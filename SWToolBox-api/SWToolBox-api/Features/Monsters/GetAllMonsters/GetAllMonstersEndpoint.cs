using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Monsters.GetAllMonsters;

[HttpGet("")]
[Group<MonstersGroup>]
[AllowAnonymous]
public class GetAllMonstersEndpoint(ISender sender) : EndpointWithoutRequest<Ok<GetAllMonstersResponse>, GetAllMonstersMapper>
{
    public override async Task<Ok<GetAllMonstersResponse>> ExecuteAsync(CancellationToken ct)
    {
        var monsters = await sender.Send(new GetAllMonstersQuery(), ct);
        var monsterResponses = monsters.Select(Map.FromEntity);
        
        return TypedResults.Ok(new GetAllMonstersResponse(monsterResponses));
    }
}
