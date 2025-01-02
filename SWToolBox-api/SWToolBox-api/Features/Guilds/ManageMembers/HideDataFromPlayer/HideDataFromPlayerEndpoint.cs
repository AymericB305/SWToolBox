using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.ManageMembers.HideDataFromPlayer;

[HttpPatch("{id:guid}/hide")]
[Group<MembersGroup>]
public class HideDataFromPlayerEndpoint(ISender sender) : Endpoint<HideDataFromPlayerCommand, Results<Ok, NotFound>>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(HideDataFromPlayerCommand req, CancellationToken ct)
    {
        var successOrNotFound = await sender.Send(req, ct);

        return successOrNotFound.Match<Results<Ok, NotFound>>(
            success => TypedResults.Ok(),
            notFound => TypedResults.NotFound()
        );
    }
}