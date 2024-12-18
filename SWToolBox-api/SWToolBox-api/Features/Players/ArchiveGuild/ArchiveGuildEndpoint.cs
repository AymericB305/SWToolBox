using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Players.ArchiveGuild;

[HttpPatch("{id:guid}/guilds/{guildId:guid}/archive")]
[Group<PlayersGroup>]
[Authorize(Policy = "ManagePlayerData")]
public class ArchiveGuildEndpoint(ISender sender) : Endpoint<ArchiveGuildCommand, Results<Ok, NotFound>>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(ArchiveGuildCommand req, CancellationToken ct)
    {
        var successOrNotFound = await sender.Send(req, ct);

        return successOrNotFound.Match<Results<Ok, NotFound>>(
            success => TypedResults.Ok(),
            notFound => TypedResults.NotFound()
        );
    }
}