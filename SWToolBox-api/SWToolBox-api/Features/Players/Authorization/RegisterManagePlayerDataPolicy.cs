using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Players.Authorization;

public static class RegisterManagePlayerDataPolicy
{
    public static AuthorizationBuilder AddManagePlayerDataPolicy(this AuthorizationBuilder builder)
    {
        return builder.AddPolicy("ManagePlayerData", policy => policy.RequireAssertion(async context =>
        {
            var httpContextAccessor = context.Resource as HttpContext;
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return false;
            }

            var routePlayerId = httpContextAccessor?.GetRouteData().Values["id"]?.ToString();
            using var scope = httpContextAccessor!.RequestServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SwDbContext>();
            var player = await dbContext.Players.FirstOrDefaultAsync(p => p.UserId == Guid.Parse(userId));
            
            return routePlayerId != null && player != null && routePlayerId == player.Id.ToString();
        }));
    }
}