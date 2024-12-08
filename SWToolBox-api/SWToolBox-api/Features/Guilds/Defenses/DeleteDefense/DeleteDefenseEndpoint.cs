using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Defenses.DeleteDefense;

[HttpDelete("{id:guid}")]
[Group<GuildDefensesGroup>]
public class DeleteDefenseEndpoint(ISender sender) : Endpoint<DeleteDefenseCommand, Results<Ok, NotFound>>
{
    public override async Task<Results<Ok, NotFound>> ExecuteAsync(DeleteDefenseCommand req, CancellationToken ct)
    {
        var successOrErrorOrNotFound = await sender.Send(req, ct);
        
        return successOrErrorOrNotFound.Match<Results<Ok, NotFound>>(
            success => TypedResults.Ok(),
            notFound => TypedResults.NotFound()
        );
    }
}