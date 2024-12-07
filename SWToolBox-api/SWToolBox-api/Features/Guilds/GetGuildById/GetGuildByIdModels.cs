using MediatR;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.GetGuildById;

public record GetGuildByIdQuery(Guid Id) : IRequest<OneOf<Guild, NotFound>>;
public record GetGuildByIdResponse(Guid Id, string Name, IEnumerable<PlayerResponse> Players);
public record PlayerResponse(Guid Id, string Name, IEnumerable<DefenseResponse> Defenses);
public record DefenseResponse(MonsterResponse MonsterLead, MonsterResponse Monster2, MonsterResponse Monster3, short Wins, short? Losses);
public record MonsterResponse(long Id, string Name);

public static class GetGuildByIdMapper
{

    public static GetGuildByIdResponse ToResponse(this Guild guild)
    {
        return new GetGuildByIdResponse(guild.Id, guild.Name, guild.GuildPlayers.Select(p => p.Player.ToResponse()));
    }

    private static PlayerResponse ToResponse(this Player player)
    {
        return new PlayerResponse(player.Id, player.Name, player.PlayerDefenses.Select(d => d.ToResponse()));
    }

    private static DefenseResponse ToResponse(this PlayerDefense playerDefense)
    {
        return new DefenseResponse(
            playerDefense.Defense.MonsterLead.ToResponse(),
            playerDefense.Defense.Monster2.ToResponse(),
            playerDefense.Defense.Monster3.ToResponse(),
            playerDefense.Wins,
            playerDefense.Losses
        );
    }

    private static MonsterResponse ToResponse(this Monster monster)
    {
        return new MonsterResponse(monster.Id, monster.Name);
    }
}