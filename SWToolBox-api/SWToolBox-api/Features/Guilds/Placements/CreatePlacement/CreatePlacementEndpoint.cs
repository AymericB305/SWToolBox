using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Placements.CreatePlacement;

[HttpPost("")]
[Group<PlacementsGroup>]
public class CreatePlacementEndpoint(ISender sender) : Endpoint<CreatePlacementCommand, Results<Ok<CreatePlacementResponse>, NotFound, ProblemDetails>>
{
    public override async Task<Results<Ok<CreatePlacementResponse>, NotFound, ProblemDetails>> ExecuteAsync(CreatePlacementCommand req, CancellationToken ct)
    {
        var placementOrFailureOrNotFound = await sender.Send(req, ct);
        
        return placementOrFailureOrNotFound.Match<Results<Ok<CreatePlacementResponse>, NotFound, ProblemDetails>>(
            placement => TypedResults.Ok(placement.ToResponse()),
            failure =>
            {
                AddError(failure.ErrorMessage);
                return new ProblemDetails();
            },
            notFound => TypedResults.NotFound()
        );
    }
}