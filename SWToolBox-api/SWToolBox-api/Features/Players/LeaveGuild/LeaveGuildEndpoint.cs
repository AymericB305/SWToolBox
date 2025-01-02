using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Players.LeaveGuild;

[HttpDelete("{id:guid}/guilds/{guildId:guid}")]
[Group<PlayersGroup>]
public class LeaveGuildEndpoint(ISender sender) : Endpoint<LeaveGuildCommand, Results<Ok, ProblemDetails>>
{
    public override async Task<Results<Ok, ProblemDetails>> ExecuteAsync(LeaveGuildCommand req, CancellationToken ct)
    {
        var successOrErrorOrNotFound = await sender.Send(req, ct);

        return successOrErrorOrNotFound.Match<Results<Ok, ProblemDetails>>(
            success => TypedResults.Ok(),
            failure =>
            {
                AddError(r => r.Id, failure.ErrorMessage);
                return new ProblemDetails(ValidationFailures);
            },
            notFound =>
            {
                AddError(r => r.Id, "Player wasn't part of this guild.");
                return new ProblemDetails(ValidationFailures);
            }
        );
    }
}