using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using NotFound = OneOf.Types.NotFound;

namespace SWToolBox_api.Features.Guilds.ManageMembers.RemovePlayerFromGuild;

public record RemovePlayerFromGuildCommand([FromRoute] Guid GuildId, [FromRoute] Guid Id, bool HideData) : IRequest<OneOf<Success, Failure, NotFound>>;
