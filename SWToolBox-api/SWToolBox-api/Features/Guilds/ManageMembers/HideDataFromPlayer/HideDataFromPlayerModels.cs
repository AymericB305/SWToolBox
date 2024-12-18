using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Guilds.ManageMembers.HideDataFromPlayer;

public record HideDataFromPlayerCommand([FromRoute] Guid GuildId, [FromRoute] Guid Id): IRequest<OneOf<Success, NotFound>>;