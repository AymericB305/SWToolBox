using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.Admin;

public class GuildAdminGroup : SubGroup<GuildsGroup>
{
    public GuildAdminGroup()
    {
        Configure("{guildId:guid}", ep =>
        {
            ep.Policies("ReadGuildData", "GuildAdmin");
        });
    }
}