using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.CreatePlayer;

public record CreatePlayerCommand(string Name) : IRequest<CreatePlayerDto>;

internal sealed class CreatePlayerHandler(SwDbContext context) : IRequestHandler<CreatePlayerCommand, CreatePlayerDto>
{
    public async Task<CreatePlayerDto> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var existingPlayer = await context.Players.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);

        if (existingPlayer is not null)
        {
            return existingPlayer.ToDto(false, $"A Player with the name {request.Name} already exists.");
        }
        
        var player = await context.AddAsync(request.ToEntity(), cancellationToken);
        
        return player.Entity.ToDto(true);
    }
}