using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.ManageDefenses;

public sealed class GuildDefensesGroup : SubGroup<GuildsGroup>
{
    public GuildDefensesGroup()
    {
        Configure("{guildId:guid}/defenses", ep =>
        {
            ep.Policies("ManageDefenses");
        });
    }
}