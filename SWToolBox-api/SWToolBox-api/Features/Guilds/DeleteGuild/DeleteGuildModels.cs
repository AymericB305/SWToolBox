using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Guilds.DeleteGuild;

public record DeleteGuildCommand([FromRoute] Guid Id) : IRequest<OneOf<Success, Error, NotFound>>;
