using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Placements.UpdatePlacementWinRate;

[HttpPatch("{id:guid}")]
[Group<PlacementsGroup>]
public class UpdatePlacementWinRateEndpoint(ISender sender) : Endpoint<UpdatePlacementWinRateCommand, Results<Ok, NotFound>>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(UpdatePlacementWinRateCommand req, CancellationToken ct)
    {
        var successOrNotFound = await sender.Send(req, ct);

        return successOrNotFound.Match<Results<Ok, NotFound>>(
            success => TypedResults.Ok(),
            notFound => TypedResults.NotFound()
        );
    }
}