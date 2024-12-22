using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Players.DeletePlayer;

public record DeletePlayerCommand([FromRoute] Guid Id) : IRequest<OneOf<Success, NotFound>>;