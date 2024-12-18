using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.Placements;

public sealed class PlacementGroup : SubGroup<GuildsGroup>
{
    public PlacementGroup()
    {
        Configure("{guildId:guid}/placements", ep =>
        {
            ep.Policies("ManageDefenses");
        });
    }
}