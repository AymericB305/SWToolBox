using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.CreatePlayer;

public class CreatePlayerHandler(SwDbContext context) : IRequestHandler<CreatePlayerCommand, OneOf<Player, Existing>>
{
    public async Task<OneOf<Player, Existing>> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var existingPlayer = await context.Players
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
        
        if (existingPlayer is not null)
        {
            return new Existing();
        }
        
        var player = await context.Players
            .AddAsync(request.ToEntity(), cancellationToken);
        
        return player.Entity;
    }
}