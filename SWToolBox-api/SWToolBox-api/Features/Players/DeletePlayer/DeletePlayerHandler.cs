using MediatR;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Players.DeletePlayer;

internal sealed class DeletePlayerHandler : IRequestHandler<DeletePlayerCommand, OneOf<Success, NotFound>>
{
    public Task<OneOf<Success, NotFound>> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}