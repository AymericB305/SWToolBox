﻿using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.ManagePlacements.UpdatePlacement;

[HttpPut("{id:guid}")]
[Group<PlacementsGroup>]
public class UpdatePlacementEndpoint(ISender sender) : Endpoint<UpdatePlacementCommand, Results<Ok<UpdatePlacementResponse>, NotFound, ProblemDetails>>
{
    public override async Task<Results<Ok<UpdatePlacementResponse>, NotFound, ProblemDetails>> ExecuteAsync(UpdatePlacementCommand req, CancellationToken ct)
    {
        var placementOrFailureOrNotFound = await sender.Send(req, ct);
        
        return placementOrFailureOrNotFound.Match<Results<Ok<UpdatePlacementResponse>, NotFound, ProblemDetails>>(
            placement => TypedResults.Ok(placement.ToResponse()),
            failure =>
            {
                AddError(failure.ErrorMessage);
                return new ProblemDetails(ValidationFailures);
            },
            notFound => TypedResults.NotFound()
        );
    }
}