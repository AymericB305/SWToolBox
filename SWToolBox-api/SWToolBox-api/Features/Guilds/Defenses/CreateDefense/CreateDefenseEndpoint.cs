﻿using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Defenses.CreateDefense;

[HttpPost("")]
[Group<GuildDefensesGroup>]
public class CreateDefenseEndpoint(ISender sender) : Endpoint<CreateDefenseCommand, Results<Ok<CreateDefenseResponse>, NotFound<string>, Conflict<string>>>
{
    public override async Task<Results<Ok<CreateDefenseResponse>, NotFound<string>, Conflict<string>>> ExecuteAsync(CreateDefenseCommand req, CancellationToken ct)
    {
        var defenseOrNotFoundOrExisting = await sender.Send(req, ct);

        return defenseOrNotFoundOrExisting.Match<Results<Ok<CreateDefenseResponse>, NotFound<string>, Conflict<string>>>(
            defense => TypedResults.Ok(defense.ToResponse()),
            notFound => TypedResults.NotFound(notFound.ErrorMessage),
            existing => TypedResults.Conflict("This defense already exists.")
        );
    }
}