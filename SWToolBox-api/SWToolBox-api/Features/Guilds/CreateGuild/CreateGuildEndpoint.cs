using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

[HttpPost("")]
[Group<GuildsGroup>]
public class CreateGuildEndpoint(ISender sender) : Endpoint<CreateGuildRequest, Ok<CreateGuildResponse>>
{
    public override async Task<Ok<CreateGuildResponse>> ExecuteAsync(CreateGuildRequest req, CancellationToken ct)
    {
        var guild = await sender.Send(req.ToCommand(), ct);
        return TypedResults.Ok(guild.ToResponse());
    }
}