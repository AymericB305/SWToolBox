using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

internal sealed class DeleteGuildHandler(SwDbContext context) : IRequestHandler<DeleteGuildCommand, OneOf<Success, NotFound>>
{
    public async Task<OneOf<Success, NotFound>> Handle(DeleteGuildCommand request, CancellationToken cancellationToken)
    {
        var guild = await context.Guilds.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        if (guild is null)
        {
            return new NotFound();
        }
        
        context.Guilds.Remove(guild);
        await context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}