using FastEndpoints;

namespace SWToolBox_api.Features.Players;

public sealed class PlayersGroup : Group
{
    public PlayersGroup()
    {
        Configure("players", ep =>
        {
            ep.AllowAnonymous();
        });
    }
}