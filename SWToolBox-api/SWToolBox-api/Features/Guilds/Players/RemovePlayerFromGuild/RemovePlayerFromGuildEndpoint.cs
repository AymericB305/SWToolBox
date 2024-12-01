using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Players.RemovePlayerFromGuild;

[HttpDelete("{id:guid}")]
[Group<GuildPlayersGroup>]
public class RemovePlayerFromGuildEndpoint(ISender sender) : Endpoint<RemovePlayerFromGuildRequest, Ok<RemovePlayerFromGuildResponse>>
{
    public override async Task<Ok<RemovePlayerFromGuildResponse>> ExecuteAsync(RemovePlayerFromGuildRequest req, CancellationToken ct)
    {
        var response = await sender.Send(req.ToCommand(), ct);
        
        return TypedResults.Ok(response.ToResponse());
    }
}