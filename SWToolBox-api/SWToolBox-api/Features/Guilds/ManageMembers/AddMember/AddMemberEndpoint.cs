using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.ManageMembers.AddMember;

[HttpPost("")]
[Group<MembersGroup>]
public class AddMemberEndpoint(ISender sender) : Endpoint<AddMemberCommand, Results<Ok<AddMemberResponse>, Conflict<string>, NotFound>>
{
    public override async Task<Results<Ok<AddMemberResponse>, Conflict<string>, NotFound>> ExecuteAsync(AddMemberCommand req, CancellationToken ct)
    {
        var playerOrExisting = await sender.Send(req, ct);

        return playerOrExisting.Match<Results<Ok<AddMemberResponse>, Conflict<string>, NotFound>>(
            player => TypedResults.Ok(player.ToResponse()),
            existing => TypedResults.Conflict($"A Player with the name {req.Name} already exists in this guild."),
            notFound => TypedResults.NotFound()
        );
    }
}