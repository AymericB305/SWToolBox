using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.Defenses;

public sealed class GuildDefensesGroup : SubGroup<GuildsGroup>
{
    public GuildDefensesGroup()
    {
        Configure("{guildId:guid}/defenses", ep =>
        {
            
        });
    }
}