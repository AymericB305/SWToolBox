using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.Placements;

public sealed class PlacementsGroup : SubGroup<GuildsGroup>
{
    public PlacementsGroup()
    {
        Configure("{guildId:guid}/placements", ep =>
        {
            ep.Policies("ManageDefenses");
        });
    }
}