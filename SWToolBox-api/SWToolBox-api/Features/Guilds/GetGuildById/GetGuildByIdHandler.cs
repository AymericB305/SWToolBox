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

        if (guild is null)
        {
            return new NotFound();
        }

        return guild;
    }
}