using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Guilds.ManagePlacements.DeletePlacement;

public record DeletePlacementCommand([FromRoute] Guid Id) : IRequest<OneOf<Success, NotFound>>;