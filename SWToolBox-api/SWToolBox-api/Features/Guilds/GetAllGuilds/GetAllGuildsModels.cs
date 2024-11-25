using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.GetAllGuilds;

public record GetAllGuildsResponse(IEnumerable<GuildResponse> Guilds);
public record GuildResponse(Guid Id, string Name);

public static class GetAllGuildsMapper
{
    public static GetAllGuildsResponse ToResponse(this IEnumerable<Guild> guilds)
    {
        return new GetAllGuildsResponse(guilds.Select(g => g.ToResponse()));
    }

    private static GuildResponse ToResponse(this Guild guild)
    {
        return new GuildResponse(guild.Id, guild.Name);
    }
}