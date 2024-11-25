using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

public record DeleteGuildCommand(Guid Id) : IRequest;

internal sealed class DeleteGuildHandler(SwDbContext context) : IRequestHandler<DeleteGuildCommand>
{
    public async Task Handle(DeleteGuildCommand request, CancellationToken cancellationToken)
    {
        await context.Guilds.Where(g => g.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}