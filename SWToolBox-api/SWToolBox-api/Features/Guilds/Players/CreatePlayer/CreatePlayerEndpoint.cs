using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Players.CreatePlayer;

[HttpPost("")]
[Group<GuildPlayersGroup>]
public class CreatePlayerEndpoint(ISender sender) : Endpoint<CreatePlayerRequest, Ok<CreatePlayerResponse>>
{
    public override async Task<Ok<CreatePlayerResponse>> ExecuteAsync(CreatePlayerRequest req, CancellationToken ct)
    {
        var player = await sender.Send(req.ToCommand(), ct);
        return TypedResults.Ok(player.ToResponse());
    }
}