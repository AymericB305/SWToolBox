using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManagePlacements.CreatePlacement;

internal sealed class CreatePlacementHandler(SwDbContext context) : IRequestHandler<CreatePlacementCommand, OneOf<Placement, Failure, NotFound>>
{
    public async Task<OneOf<Placement, Failure, NotFound>> Handle(CreatePlacementCommand request, CancellationToken cancellationToken)
    {
        var guild = await context.Guilds
            .FirstOrDefaultAsync(g => g.Id == request.GuildId, cancellationToken);
        
        if (guild is null)
        {
            return new NotFound();
        }
        
        var player = await context.Players
            .FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);
        
        if (player is null)
        {
            return new Failure("Player not found");
        }
        
        var tower = await context.Towers
            .Include(t => t.Placements)
            .FirstOrDefaultAsync(t => t.Id == request.TowerId, cancellationToken);
        
        if (tower is null)
        {
            return new Failure("Tower not found");
        }

        if (tower.Placements.Count >= 5)
        {
            return new Failure("This tower already contains too many defenses");
        }
        
        var defense = await context.Defenses
            .FirstOrDefaultAsync(d => d.Id == request.DefenseId, cancellationToken);
        
        if (defense is null)
        {
            return new Failure("Defense not found");
        }

        if (defense.GuildId != request.GuildId)
        {
            return new Failure("This defense doesn't belong to this guild");
        }
        
        var placement = await context.Placements
            .AddAsync(request.ToEntity(), cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return await context.Placements
            .Include(p => p.Player)
            .Include(p => p.Tower)
                .ThenInclude(t => t.Team)
            .Include(p => p.Defense)
                .ThenInclude(d => d.MonsterLead)
            .Include(p => p.Defense)
                .ThenInclude(d => d.Monster2)
            .Include(p => p.Defense)
                .ThenInclude(d => d.Monster3)
            .FirstAsync(p => p.Id == placement.Entity.Id, cancellationToken);
    }
}