using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.ManageMembers.RemovePlayerFromGuild;

[HttpDelete("{id:guid}")]
[Group<MembersGroup>]
public class RemovePlayerFromGuildEndpoint(ISender sender) : Endpoint<RemovePlayerFromGuildCommand, Results<Ok, ProblemDetails>>
{
    public override async Task<Results<Ok, ProblemDetails>> ExecuteAsync(RemovePlayerFromGuildCommand req, CancellationToken ct)
    {
        var successOrErrorOrNotFound = await sender.Send(req, ct);

        return successOrErrorOrNotFound.Match<Results<Ok, ProblemDetails>>(
            success => TypedResults.Ok(),
            failure =>
            {
                AddError(r => r.Id, failure.ErrorMessage);
                return new ProblemDetails();
            },
            notFound =>
            {
                AddError(r => r.Id, "Player wasn't part of this guild.");
                return new ProblemDetails();
            }
        );
    }
}