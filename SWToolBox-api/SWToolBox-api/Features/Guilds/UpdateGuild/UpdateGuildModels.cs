using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.UpdateGuild;

public record UpdateGuildRequest(Guid Id, string Name);
public record UpdateGuildDto(Guid Id, string Name, bool IsSuccess);
public record UpdateGuildResponse(Guid Id, string Name, bool IsSuccess);

public static class UpdateGuildMapper
{
    public static UpdateGuildCommand ToCommand(this UpdateGuildRequest request)
    {
        return new UpdateGuildCommand(request.Id, request.Name);
    }

    public static UpdateGuildDto ToDto(this Guild guild, bool isSuccess)
    {
        return new UpdateGuildDto(guild.Id, guild.Name, isSuccess);
    }
    
    public static UpdateGuildResponse ToResponse(this UpdateGuildDto dto)
    {
        return new UpdateGuildResponse(dto.Id, dto.Name, dto.IsSuccess);
    }
}