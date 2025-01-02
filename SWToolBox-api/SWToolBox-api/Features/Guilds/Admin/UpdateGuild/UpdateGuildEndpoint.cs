using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Admin.UpdateGuild;

[HttpPut("")]
[Group<GuildAdminGroup>]
public class UpdateGuildEndpoint(ISender sender) : Endpoint<UpdateGuildCommand, Results<Ok<UpdateGuildResponse>, NotFound, Conflict<string>>>
{
    public override async Task<Results<Ok<UpdateGuildResponse>, NotFound, Conflict<string>>> ExecuteAsync(UpdateGuildCommand req, CancellationToken ct)
    {
        var guildOrNotFoundOrExisting = await sender.Send(req, ct);

        return guildOrNotFoundOrExisting.Match<Results<Ok<UpdateGuildResponse>, NotFound, Conflict<string>>>(
            guild => TypedResults.Ok(guild.ToResponse()),
            notFound => TypedResults.NotFound(),
            existing => TypedResults.Conflict($"A Guild with the name {req.Name} already exists.")
        );
    }
}