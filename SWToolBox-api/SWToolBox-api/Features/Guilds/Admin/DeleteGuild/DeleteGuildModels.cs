using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Guilds.Admin.DeleteGuild;

public record DeleteGuildCommand([FromRoute] Guid GuildId) : IRequest<OneOf<Success, NotFound>>;
