using FastEndpoints;
using FluentValidation;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.CreatePlayer;

public record CreatePlayerRequest(string Name);
public record CreatePlayerDto(Guid Id, string Name, bool IsSuccess, string? ErrorMessage);
public record CreatePlayerResponse(Guid Id, string Name, bool IsSuccess, string? ErrorMessage);

public class CreatePlayerValidator : Validator<CreatePlayerRequest>
{
    public CreatePlayerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name of the player is required.")
            .Must(x => x.Length is > 5 and < 50)
            .WithMessage("The name of the player must be between 5 and 50 characters.");
    }
}

public static class CreatePlayerMapper
{
    public static CreatePlayerCommand ToCommand(this CreatePlayerRequest request)
    {
        return new CreatePlayerCommand(request.Name);
    }
    
    public static Player ToEntity(this CreatePlayerCommand command)
    {
        return new Player
        {
            Name = command.Name,
        };
    }

    public static CreatePlayerDto ToDto(this Player player, bool isSuccess, string? errorMessage = null)
    {
        return new CreatePlayerDto(player.Id, player.Name, isSuccess, errorMessage);
    }

    public static CreatePlayerResponse ToResponse(this CreatePlayerDto dto)
    {
        return new CreatePlayerResponse(dto.Id, dto.Name, dto.IsSuccess, dto.ErrorMessage);
    }
}