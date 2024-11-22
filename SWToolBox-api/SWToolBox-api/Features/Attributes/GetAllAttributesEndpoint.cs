using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Attributes;

[HttpGet("api/attribute")]
[AllowAnonymous]
public class GetAllAttributesEndpoint : EndpointWithoutRequest<IEnumerable<LeaderSkill>>
{
    public SwDbContext Context { get; set; }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(Context.LeaderSkills.ToList(), cancellation: ct);
    }
}
