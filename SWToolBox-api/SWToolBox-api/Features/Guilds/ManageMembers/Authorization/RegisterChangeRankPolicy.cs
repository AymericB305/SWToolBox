using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.ManageMembers.Authorization;

public static class RegisterChangeRankPolicy
{
    public static AuthorizationBuilder AddChangeRankPolicy(this AuthorizationBuilder builder)
    {
        return builder.AddPolicy("ChangeRank", policy => policy.RequireAssertion(async context =>
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
            if (guild is null)
            {
                return false;
            }
            
            var routeRankId = httpContextAccessor?.GetRouteData()?.Values["rankId"]?.ToString();
            if (routeRankId is null)
            {
                return false;
            }

            var guildRankId = guild.RankId;

            return guildRankId > long.Parse(routeRankId);
        }));
    }
}