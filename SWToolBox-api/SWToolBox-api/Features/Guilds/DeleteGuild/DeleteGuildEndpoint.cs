using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

[HttpDelete("{guildId:guid}")]
[Group<GuildsGroup>]
[Authorize(Policy = "GuildAdmin")]
public class DeleteGuildEndpoint(ISender sender) : Endpoint<DeleteGuildCommand, Results<Ok, NotFound>>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(DeleteGuildCommand req, CancellationToken ct)
    {
        var successOrErrorOrNotFound = await sender.Send(req, ct);

        return successOrErrorOrNotFound.Match<Results<Ok, NotFound>>(
            success => TypedResults.Ok(),
            notFound => TypedResults.NotFound()
        );
    }
}