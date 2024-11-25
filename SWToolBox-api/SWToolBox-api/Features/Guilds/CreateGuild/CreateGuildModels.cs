using FastEndpoints;
using FluentValidation;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.CreateGuild;

public record CreateGuildRequest(string Name);
public record CreateGuildResponse(Guid Id, string Name);

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
    public static CreateGuildCommand ToCommand(this CreateGuildRequest r)
    {
        return new CreateGuildCommand(r.Name);
    }
    
    public static Guild ToEntity(this CreateGuildCommand r)
    {
        return new Guild()
        {
            Name = r.Name,
        };
    }

    public static CreateGuildResponse ToResponse(this Guild e)
    {
        return new CreateGuildResponse(e.Id, e.Name);
    }
}