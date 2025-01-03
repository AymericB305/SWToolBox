﻿using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.GetAllGuilds;

[HttpGet("")]
[Group<GuildsGroup>]
public class GetAllGuildsEndpoint(ISender sender) : EndpointWithoutRequest<Ok<GetAllGuildsResponse>>
{
    public override async Task<Ok<GetAllGuildsResponse>> ExecuteAsync(CancellationToken ct)
    {
        var guilds = await sender.Send(new GetAllGuildsQuery(), ct);

        return TypedResults.Ok(guilds.ToResponse());
    }
}