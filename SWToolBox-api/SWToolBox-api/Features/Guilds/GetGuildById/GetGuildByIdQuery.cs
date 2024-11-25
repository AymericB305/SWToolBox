using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.GetGuildById;

public record GetGuildByIdQuery(Guid Id) : IRequest<Guild?>;

internal sealed class GetGuildByIdHandler(SwDbContext context) : IRequestHandler<GetGuildByIdQuery, Guild?>
{
    public async Task<Guild?> Handle(GetGuildByIdQuery request, CancellationToken cancellationToken)
    {
        return await context
            .Guilds
            .Include(g => g.GuildPlayers)
                .ThenInclude(gp => gp.Player)
                    .ThenInclude(p => p.PlayerDefenses)
                        .ThenInclude(pd => pd.Defense)
                           .ThenInclude(pd => pd.MonsterLead)
            .Include(g => g.GuildPlayers)
                .ThenInclude(gp => gp.Player)
                    .ThenInclude(p => p.PlayerDefenses)
                        .ThenInclude(pd => pd.Defense)
                            .ThenInclude(pd => pd.Monster2)
            .Include(g => g.GuildPlayers)
                .ThenInclude(gp => gp.Player)
                    .ThenInclude(p => p.PlayerDefenses)
                        .ThenInclude(pd => pd.Defense)
                            .ThenInclude(pd => pd.Monster3)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
    }
}