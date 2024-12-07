using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;

namespace SWToolBox_api.Features.Guilds.Players.RemovePlayerFromGuild;

public record RemovePlayerFromGuildCommand([FromRoute] Guid GuildId, [FromRoute] Guid Id) : IRequest<OneOf<Success, Failure, NotFound>>;
