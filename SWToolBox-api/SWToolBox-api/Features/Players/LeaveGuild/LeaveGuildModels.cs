using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;

namespace SWToolBox_api.Features.Players.LeaveGuild;

public record LeaveGuildCommand([FromRoute] Guid Id, [FromRoute] Guid GuildId, bool ArchiveGuild) : IRequest<OneOf<Success, Failure, NotFound>>;