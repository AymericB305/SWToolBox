using FastEndpoints;
using MediatR;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Monsters.GetAllMonsters;

public record GetAllMonstersQuery : IRequest<IEnumerable<Monster>>;

public record GetAllMonstersResponse(IEnumerable<MonsterResponse> Monsters);
public record MonsterResponse(long Id, string Name, string Element, MonsterResponseLeaderSkill? LeaderSkill);
public record MonsterResponseLeaderSkill(Guid Id, int Value, string LeaderType, string Area);

public static class GetAllMonstersMapper
{
    public static GetAllMonstersResponse ToResponse(this IEnumerable<Monster> monsters)
    {
        return new GetAllMonstersResponse(monsters.Select(m => m.ToResponse()));
    }
    
    private static MonsterResponse ToResponse(this Monster monster)
    {
        MonsterResponseLeaderSkill? leaderSkill = null;

        if (monster.Leader is null)
        {
            return new MonsterResponse(
                monster.Id,
                monster.Name,
                monster.Element.Name,
                leaderSkill
            );
        }

        var area = monster.Leader.LeaderType.Name == "Element"
            ? monster.Element.Name
            : monster.Leader.LeaderType.Name;
            
        leaderSkill = new MonsterResponseLeaderSkill(
            monster.Leader.Id,
            monster.Leader.Value,
            monster.Leader.LeaderType.Name,
            area
        );

        return new MonsterResponse(
            monster.Id,
            monster.Name,
            monster.Element.Name,
            leaderSkill
        );
    }
} 