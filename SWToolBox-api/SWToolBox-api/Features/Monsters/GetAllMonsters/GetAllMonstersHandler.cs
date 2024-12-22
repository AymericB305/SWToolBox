using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Monsters.GetAllMonsters;

internal sealed class GetAllMonstersHandler(SwDbContext context)
    : IRequestHandler<GetAllMonstersQuery, IEnumerable<Monster>>
{
    public async Task<IEnumerable<Monster>> Handle(GetAllMonstersQuery request, CancellationToken cancellationToken)
    {
        return await context
            .Monsters
            .Include(m => m.Leader)
                .ThenInclude(l => l!.LeaderType)
            .Include(m => m.Leader)
                .ThenInclude(l => l!.Area)
            .Include(m => m.Element)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}