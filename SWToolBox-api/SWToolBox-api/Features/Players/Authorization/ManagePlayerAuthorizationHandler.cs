using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Players.Authorization;

public class ManagePlayerAuthorizationHandler(IHttpContextAccessor httpContextAccessor, SwDbContext dbContext)
    : AuthorizationHandler<ManagePlayerDataRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ManagePlayerDataRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            context.Fail();
            return;
        }
        
        var routePlayerId = httpContextAccessor.HttpContext?.GetRouteData()?.Values["id"]?.ToString();
        var player = await dbContext.Players.FirstOrDefaultAsync(p => p.UserId == Guid.Parse(userId));
        if (routePlayerId is null || player is null || routePlayerId != player.Id.ToString())
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}