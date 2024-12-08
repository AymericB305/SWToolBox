using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.Players.CreatePlayer;

[HttpPost("")]
[Group<GuildPlayersGroup>]
public class CreatePlayerEndpoint(ISender sender) : Endpoint<CreatePlayerCommand, Results<Ok<CreatePlayerResponse>, Conflict<string>>>
{
    public override async Task<Results<Ok<CreatePlayerResponse>, Conflict<string>>> ExecuteAsync(CreatePlayerCommand req, CancellationToken ct)
    {
        var playerOrExisting = await sender.Send(req, ct);

        return playerOrExisting.Match<Results<Ok<CreatePlayerResponse>, Conflict<string>>>(
            player => TypedResults.Ok(player.ToResponse()),
            existing => TypedResults.Conflict($"A Player with the name {req.Name} already exists in this guild.")
        );
    }
}