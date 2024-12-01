using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

[HttpDelete("${id:guid}")]
[Group<GuildsGroup>]
public class DeleteGuildEndpoint(ISender sender) : Endpoint<DeleteGuildRequest, Ok<DeleteGuildResponse>>
{
    public override async Task<Ok<DeleteGuildResponse>> ExecuteAsync(DeleteGuildRequest req, CancellationToken ct)
    {
        var isSuccess = await sender.Send(req.ToCommand(), ct);
        
        string? errorMessage = isSuccess
            ? null
            : "Guild could not be deleted.";
        
        return TypedResults.Ok(new DeleteGuildResponse(isSuccess, errorMessage));
    }
}