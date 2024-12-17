using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.Authorization;

public class ReadGuildAuthorizationHandler(IHttpContextAccessor httpContextAccessor, SwDbContext dbContext)
    : AuthorizationHandler<ReadGuildDataRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadGuildDataRequirement requirement)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            context.Fail();
            return;
        }
        
        var routeGuildId = httpContextAccessor.HttpContext?.GetRouteData()?.Values["guildId"]?.ToString();
        var player = await dbContext.Players
            .Include(p => p.GuildPlayers)
            .FirstOrDefaultAsync(p => p.UserId == Guid.Parse(userId));
        
        if (routeGuildId is null || player is null)
        {
            context.Fail();
            return;
        }
        
        var guildIds = player.GuildPlayers
            .Where(gp => gp.LeftAt is null)
            .Select(p => p.GuildId.ToString())
            .ToList();
        if (!guildIds.Contains(routeGuildId))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}