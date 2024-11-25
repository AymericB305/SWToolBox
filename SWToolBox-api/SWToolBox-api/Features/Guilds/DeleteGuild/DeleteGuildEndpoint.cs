using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

[Group<GuildsGroup>]
[HttpDelete("${id:guid}")]
public class DeleteGuildEndpoint(ISender sender) : Endpoint<DeleteGuildRequest, EmptyHttpResult>
{
    public override async Task<EmptyHttpResult> ExecuteAsync(DeleteGuildRequest req, CancellationToken ct)
    {
        await sender.Send(req.ToCommand(), ct);
        
        return TypedResults.Empty;
    }
}