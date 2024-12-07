using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

internal sealed class DeleteGuildHandler(SwDbContext context) : IRequestHandler<DeleteGuildCommand, OneOf<Success, Error, NotFound>>
{
    public async Task<OneOf<Success, Error, NotFound>> Handle(DeleteGuildCommand request, CancellationToken cancellationToken)
    {
        var guild = await context.Guilds.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        if (guild is null)
        {
            return new NotFound();
        }
        
        int rowsDeleted = await context.Guilds.Where(g => g.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return rowsDeleted > 0
            ? new Success()
            : new Error();
    }
}