using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.GetGuildById;

internal sealed class GetGuildByIdHandler(SwDbContext context) : IRequestHandler<GetGuildByIdQuery, OneOf<Guild, NotFound>>
{
    public async Task<OneOf<Guild, NotFound>> Handle(GetGuildByIdQuery request, CancellationToken cancellationToken)
    {
        var guild = await context
            .Guilds
            .Include(g => g.GuildPlayers)
                .ThenInclude(gp => gp.Player)
            .Include(g => g.Defenses)
                .ThenInclude(d => d.MonsterLead)
            .Include(g => g.Defenses)
                .ThenInclude(d => d.Monster2)
            .Include(g => g.Defenses)
                .ThenInclude(pd => pd.Monster3)
            .Include(g => g.Defenses)
                .ThenInclude(d => d.Placements)
                    .ThenInclude(pdt => pdt.Tower)
                        .ThenInclude(t => t.Team)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.GuildId, cancellationToken);

        if (guild is null)
        {
            return new NotFound();
        }

        return guild;
    }
}