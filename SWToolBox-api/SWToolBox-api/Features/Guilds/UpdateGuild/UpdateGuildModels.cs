using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.UpdateGuild;

public record UpdateGuildRequest(Guid Id, string Name);
public record UpdateGuildResponse(Guid Id, string Name);

public static class UpdateGuildMapper
{
    public static UpdateGuildCommand ToCommand(this UpdateGuildRequest request)
    {
        return new UpdateGuildCommand(request.Id, request.Name);
    }
    
    public static UpdateGuildResponse ToResponse(this Guild guild)
    {
        return new UpdateGuildResponse(guild.Id, guild.Name);
    }
}