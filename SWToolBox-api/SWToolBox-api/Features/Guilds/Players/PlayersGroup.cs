using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.Players;

public sealed class PlayersGroup : SubGroup<GuildsGroup>
{
    public PlayersGroup()
    {
        Configure("{guildId:guid}/players", ep =>
        {
            ep.AllowAnonymous();
        });
    }
}