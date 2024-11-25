using FastEndpoints;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Monsters.GetAllMonsters;

public record GetAllMonstersResponse(IEnumerable<MonsterResponse> Monsters);
public record MonsterResponse(long Id, string Name, string Attribute, MonsterResponseLeaderSkill? LeaderSkill);
public record MonsterResponseLeaderSkill(Guid Id, int Value, string Type, string LeaderType);

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
                monster.Attribute.Name,
                leaderSkill
            );
        }

        var leaderType = monster.Leader.LeaderType.Name == "Element"
            ? monster.Attribute.Name
            : monster.Leader.LeaderType.Name;
            
        leaderSkill = new MonsterResponseLeaderSkill(
            monster.Leader.Id,
            monster.Leader.Value,
            monster.Leader.Type.Name,
            leaderType
        );

        return new MonsterResponse(
            monster.Id,
            monster.Name,
            monster.Attribute.Name,
            leaderSkill
        );
    }
} 