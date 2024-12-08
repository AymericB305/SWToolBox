using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Defenses.UpdateDefense;

[HttpPut("{id:guid}")]
[Group<GuildDefensesGroup>]
public class UpdateDefenseEndpoint(ISender sender) : Endpoint<UpdateDefenseCommand, Results<Ok<UpdateDefenseResponse>, NotFound>>
{
    public override async Task<Results<Ok<UpdateDefenseResponse>, NotFound>> ExecuteAsync(UpdateDefenseCommand req, CancellationToken ct)
    {
        var guildDefenseOrNotFound = await sender.Send(req, ct);

        return guildDefenseOrNotFound.Match<Results<Ok<UpdateDefenseResponse>, NotFound>>(
            guildDefense => TypedResults.Ok(guildDefense.ToResponse()),
            notFound => TypedResults.NotFound()
        );
    }
}