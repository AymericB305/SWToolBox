using Microsoft.AspNetCore.Mvc;

namespace SWToolBox_api.Features.Guilds.Players.RemovePlayerFromGuild;

public record RemovePlayerFromGuildRequest([FromRoute] Guid GuildId, [FromRoute] Guid Id);
public record RemovePlayerFromGuildResponse(string Message);

public static class RemovePlayerFromGuildMapper
{
    public static RemovePlayerFromGuildCommand ToCommand(this RemovePlayerFromGuildRequest request)
    {
        return new RemovePlayerFromGuildCommand(request.GuildId, request.Id);
    }

    public static RemovePlayerFromGuildResponse ToResponse(this string message)
    {
        return new RemovePlayerFromGuildResponse(message);
    }
}