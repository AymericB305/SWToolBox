using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.ManageMembers;

public sealed class MembersGroup : SubGroup<GuildsGroup>
{
    public MembersGroup()
    {
        Configure("{guildId:guid}/players", ep =>
        {
            ep.Policies("ReadGuildData", "ManageMembers");
        });
    }
}