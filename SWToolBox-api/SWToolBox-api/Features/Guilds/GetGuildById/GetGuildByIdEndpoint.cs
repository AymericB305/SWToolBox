using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.GetGuildById;

[Group<GuildsGroup>]
[HttpGet("${id:guid}")]
public class GetGuildByIdEndpoint(ISender sender) : Endpoint<GetGuildByIdRequest, Results<Ok<GetGuildByIdResponse>, NotFound>>
{
    public override async Task<Results<Ok<GetGuildByIdResponse>, NotFound>> ExecuteAsync(GetGuildByIdRequest req, CancellationToken ct)
    {
        var guild = await sender.Send(req.ToQuery(), ct);

        if (guild is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(guild.ToResponse());
    }
}