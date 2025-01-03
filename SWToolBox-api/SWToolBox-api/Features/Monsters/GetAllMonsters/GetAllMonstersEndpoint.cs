﻿using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Monsters.GetAllMonsters;

[HttpGet("")]
[Group<MonstersGroup>]
public class GetAllMonstersEndpoint(ISender sender) : EndpointWithoutRequest<Ok<GetAllMonstersResponse>>
{
    public override async Task<Ok<GetAllMonstersResponse>> ExecuteAsync(CancellationToken ct)
    {
        var monsters = await sender.Send(new GetAllMonstersQuery(), ct);
        
        return TypedResults.Ok(monsters.ToResponse());
    }
}
