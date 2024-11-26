using FastEndpoints;
using FluentValidation;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

public record CreateGuildRequest(string Name);

public record CreateGuildDto(Guid Id, string Name, bool IsSuccess, string? ErrorMessage);
public record CreateGuildResponse(Guid Id, string Name, bool IsSuccess, string? ErrorMessage);

public class CreateGuildValidator : Validator<CreateGuildRequest>
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
    public static CreateGuildCommand ToCommand(this CreateGuildRequest request)
    {
        return new CreateGuildCommand(request.Name);
    }
    
    public static Guild ToEntity(this CreateGuildCommand command)
    {
        return new Guild
        {
            Name = command.Name,
        };
    }

    public static CreateGuildDto ToDto(this Guild guild, bool isSuccess, string? errorMessage = null)
    {
        return new CreateGuildDto(guild.Id, guild.Name, isSuccess, errorMessage);
    }

    public static CreateGuildResponse ToResponse(this CreateGuildDto dto)
    {
        return new CreateGuildResponse(dto.Id, dto.Name, dto.IsSuccess, dto.ErrorMessage);
    }
}