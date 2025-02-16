using FastEndpoints;

namespace SWToolBox_api.Features.Guilds.ManageMembers;

public sealed class MembersGroup : SubGroup<GuildsGroup>
{
    public MembersGroup()
    {
        Configure("{guildId:guid}/members", ep =>
        {
            ep.Policies("ReadGuildData", "ManageMembers");
        });
    }
}