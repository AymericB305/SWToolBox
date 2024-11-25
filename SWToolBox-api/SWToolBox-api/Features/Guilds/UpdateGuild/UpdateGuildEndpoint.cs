using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SWToolBox_api.Features.Guilds.UpdateGuild;

[Group<GuildsGroup>]
[HttpPut("{id:guid}")]
public class UpdateGuildEndpoint(ISender sender) : Endpoint<UpdateGuildRequest, Results<Ok<UpdateGuildResponse>, NotFound>>
{
    public override async Task<Results<Ok<UpdateGuildResponse>, NotFound>> ExecuteAsync(UpdateGuildRequest req, CancellationToken ct)
    {
        var guild = await sender.Send(req.ToCommand(), ct);

        if (guild is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(guild.ToResponse());
    }
}