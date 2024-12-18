using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Players.ArchiveGuild;

public record ArchiveGuildCommand([FromRoute] Guid Id, [FromRoute] Guid GuildId) : IRequest<OneOf<Success, NotFound>>;