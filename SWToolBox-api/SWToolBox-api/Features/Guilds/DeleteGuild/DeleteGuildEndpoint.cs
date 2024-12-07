using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

[HttpDelete("${id:guid}")]
[Group<GuildsGroup>]
public class DeleteGuildEndpoint(ISender sender) : Endpoint<DeleteGuildCommand, Results<Ok, NotFound, ProblemDetails>>
{
    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(DeleteGuildCommand req, CancellationToken ct)
    {
        var successOrErrorOrNotFound = await sender.Send(req, ct);

        return successOrErrorOrNotFound.Match<Results<Ok, NotFound, ProblemDetails>>(
            success => TypedResults.Ok(),
            error =>
            {
                AddError(r => r.Id, "Guild could not be deleted.");
                return new ProblemDetails();
            },
            notFound => TypedResults.NotFound()
        );
    }
}