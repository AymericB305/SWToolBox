using MediatR;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;
using SWToolBox_api.Features.Guilds.Defenses.CreateDefense;

namespace SWToolBox_api.Features.Guilds.GetGuildById;

public record GetGuildByIdQuery(Guid Id) : IRequest<OneOf<Guild, NotFound>>;
public record GetGuildByIdResponse(Guid Id, string Name, IEnumerable<PlayerResponse> Players, IEnumerable<GuildDefenseResponse> Defenses);
public record GuildDefenseResponse(DefenseResponse Defense, string Description);
public record PlayerResponse(Guid Id, string Name, IEnumerable<PlayerDefenseResponse> Defenses);
public record PlayerDefenseResponse(DefenseResponse Defense, short Wins, short? Losses);
public record DefenseResponse(MonsterResponse MonsterLead, MonsterResponse Monster2, MonsterResponse Monster3);
public record MonsterResponse(long Id, string Name);

public static class GetGuildByIdMapper
{
    public static GetGuildByIdResponse ToResponse(this Guild guild)
    {
        return new GetGuildByIdResponse(
            guild.Id,
            guild.Name,
            guild.GuildPlayers.Select(gp => gp.Player.ToResponse()),
            guild.GuildDefenses.Select(gd => gd.ToResponse())
        );
    }

    private static PlayerResponse ToResponse(this Player player)
    {
        return new PlayerResponse(
            player.Id,
            player.Name,
            player.PlayerDefenses.Select(d => d.ToResponse())
        );
    }

    private static PlayerDefenseResponse ToResponse(this PlayerDefense playerDefense)
    {
        return new PlayerDefenseResponse(
            playerDefense.Defense.ToResponse(),
            playerDefense.Wins,
            playerDefense.Losses
        );
    }

    private static GuildDefenseResponse ToResponse(this GuildDefense guildDefense)
    {
        return new GuildDefenseResponse(guildDefense.Defense.ToResponse(), guildDefense.Description);
    }

    private static DefenseResponse ToResponse(this Defense defense)
    {
        return new DefenseResponse(
            defense.MonsterLead.ToResponse(),
            defense.Monster2.ToResponse(),
            defense.Monster3.ToResponse()
        );
    }

    private static MonsterResponse ToResponse(this Monster monster)
    {
        return new MonsterResponse(monster.Id, monster.Name);
    }
}