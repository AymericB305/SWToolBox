using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.Players;

public sealed class GuildPlayersGroup : SubGroup<GuildsGroup>
{
    public GuildPlayersGroup()
    {
        Configure("{guildId:guid}/players", ep =>
        {
            
        });
    }
}