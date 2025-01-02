using MediatR;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;
using NotFound = OneOf.Types.NotFound;

namespace SWToolBox_api.Features.Guilds.Admin.UpdateGuild;

public record UpdateGuildCommand(Guid GuildId, string Name) : IRequest<OneOf<Guild, NotFound, Existing>>;

public record UpdateGuildResponse(Guid Id, string Name);

public static class UpdateGuildMapper
{
    public static UpdateGuildResponse ToResponse(this Guild guild)
    {
        return new UpdateGuildResponse(guild.Id, guild.Name);
    }
}