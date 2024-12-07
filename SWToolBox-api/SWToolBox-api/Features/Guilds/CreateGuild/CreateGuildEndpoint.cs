using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

[HttpPost("")]
[Group<GuildsGroup>]
public class CreateGuildEndpoint(ISender sender) : Endpoint<CreateGuildCommand, Results<Ok<CreateGuildResponse>, Conflict<string>>>
{
    public override async Task<Results<Ok<CreateGuildResponse>, Conflict<string>>> ExecuteAsync(CreateGuildCommand req, CancellationToken ct)
    {
        var guildOrExisting = await sender.Send(req, ct);

        return guildOrExisting.Match<Results<Ok<CreateGuildResponse>, Conflict<string>>>(
            guild => TypedResults.Ok(guild.ToResponse()),
            existing => TypedResults.Conflict($"A Guild with the name {req.Name} already exists.")
        );
    }
}