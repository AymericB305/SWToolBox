using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Placements.DeletePlacement;

[HttpDelete("{id:guid}")]
public class DeletePlacementEndpoint(ISender sender) : Endpoint<DeletePlacementCommand, Results<Ok, NotFound>>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(DeletePlacementCommand req, CancellationToken ct)
    {
        var successOrNotFound = await sender.Send(req, ct);
        
        return successOrNotFound.Match<Results<Ok, NotFound>>(
            success => TypedResults.Ok(),
            notFound => TypedResults.NotFound()
        );
    }
}