using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.GetGuildById;

[HttpGet("{guildId:guid}")]
[Group<GuildsGroup>]
[Authorize(Policy = "ReadGuildData")]
public class GetGuildByIdEndpoint(ISender sender) : Endpoint<GetGuildByIdQuery, Results<Ok<GetGuildByIdResponse>, NotFound>>
{
    public override async Task<Results<Ok<GetGuildByIdResponse>, NotFound>> ExecuteAsync(GetGuildByIdQuery req, CancellationToken ct)
    {
        var guildOrNotFound = await sender.Send(req, ct);

        return guildOrNotFound.Match<Results<Ok<GetGuildByIdResponse>, NotFound>>(
            guild => TypedResults.Ok(guild.ToResponse()),
            notfound => TypedResults.NotFound()
        );
    }
}