using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.GetAllGuilds;

internal sealed class GetAllGuildsHandler(SwDbContext context)
    : IRequestHandler<GetAllGuildsQuery, IEnumerable<Guild>>
{
    public async Task<IEnumerable<Guild>> Handle(GetAllGuildsQuery request, CancellationToken cancellationToken)
    {
        return await context.Guilds.ToListAsync(cancellationToken);
    }
}