using MediatR;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

public record DeleteGuildCommand(Guid Id) : IRequest<bool>;

internal sealed class DeleteGuildHandler(SwDbContext context) : IRequestHandler<DeleteGuildCommand, bool>
{
    public async Task<bool> Handle(DeleteGuildCommand request, CancellationToken cancellationToken)
    {
        int rowsDeleted = await context.Guilds.Where(g => g.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return rowsDeleted > 0;
    }
}