using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.ManagePlacements;

public sealed class PlacementsGroup : SubGroup<GuildsGroup>
{
    public PlacementsGroup()
    {
        Configure("{guildId:guid}/placements", ep =>
        {
            ep.Policies("ReadGuildData", "ManageDefenses");
        });
    }
}