using FastEndpoints;
using FluentValidation;
using MediatR;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

public record CreateGuildCommand(string Name, Guid FounderId) : IRequest<OneOf<Guild, Existing>>;

public record CreateGuildResponse(Guid Id, string Name);

public class CreateGuildValidator : Validator<CreateGuildCommand>
{
    public CreateGuildValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name of the guild is required.")
            .Must(x => x.Length is > 5 and < 50)
            .WithMessage("The name of the guild must be between 5 and 50 characters.");
    }
}

public static class CreateGuildMapper
{
    public static Guild ToEntity(this CreateGuildCommand command)
    {
        return new Guild
        {
            Name = command.Name,
        };
    }

    public static CreateGuildResponse ToResponse(this Guild guild)
    {
        return new CreateGuildResponse(guild.Id, guild.Name);
    }
}