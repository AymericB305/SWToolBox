using FastEndpoints;

namespace SWToolBox_api.Features.Guilds;

public sealed class GuildsGroup : Group
{
    public GuildsGroup()
    {
        Configure("guilds", ep =>
        {
            
        });
    }
}