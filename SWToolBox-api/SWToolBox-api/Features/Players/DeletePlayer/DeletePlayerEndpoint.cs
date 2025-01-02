using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Players.DeletePlayer;

[HttpDelete("{id:guid}")]
[Group<PlayersGroup>]
public class DeletePlayerEndpoint(ISender sender) : Endpoint<DeletePlayerCommand, Results<Ok, NotFound>>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(DeletePlayerCommand req, CancellationToken ct)
    {
        var successOrNotFound = await sender.Send(req, ct);
        
        return successOrNotFound.Match<Results<Ok, NotFound>>(
            success => TypedResults.Ok(),
            notFound => TypedResults.NotFound()
        );
    }
}