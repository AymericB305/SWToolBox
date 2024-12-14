using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Defenses.UpdateDefense;

[HttpPut("{id:guid}")]
[Group<GuildDefensesGroup>]
public class UpdateDefenseEndpoint(ISender sender) : Endpoint<UpdateDefenseCommand, Results<Ok<UpdateDefenseResponse>, NotFound, Conflict<string>>>
{
    public override async Task<Results<Ok<UpdateDefenseResponse>, NotFound, Conflict<string>>> ExecuteAsync(UpdateDefenseCommand req, CancellationToken ct)
    {
        var defenseOrNotFound = await sender.Send(req, ct);

        return defenseOrNotFound.Match<Results<Ok<UpdateDefenseResponse>, NotFound, Conflict<string>>>(
            guildDefense => TypedResults.Ok(guildDefense.ToResponse()),
            notFound => TypedResults.NotFound(),
            existing => TypedResults.Conflict("This defenses already exists.")
        );
    }
}