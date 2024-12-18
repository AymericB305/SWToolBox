using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.Authorization;

public static class RegisterManageDefensesPolicy
{
    public static AuthorizationBuilder AddManageDefensesPolicy(this AuthorizationBuilder builder)
    {
        return builder.AddPolicy("ManageDefenses", policy => policy.RequireAssertion(async context =>
        {
            var httpContextAccessor = context.Resource as HttpContext;
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return false;
            }

            var routeGuildId = httpContextAccessor?.GetRouteData()?.Values["guildId"]?.ToString();
            if (routeGuildId is null)
            {
                return false;
            }

            using var scope = httpContextAccessor!.RequestServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SwDbContext>();

            var player = await dbContext.Players
                .Include(p => p.GuildPlayers)
                .FirstOrDefaultAsync(p => p.UserId == Guid.Parse(userId));

            var guild = player?.GuildPlayers
                .FirstOrDefault(gp => gp.GuildId == Guid.Parse(routeGuildId));

            var guildRankId = guild?.RankId;

            return guildRankId > 1;
        }));
    }
}