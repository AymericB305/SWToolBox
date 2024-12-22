using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Players.UpdatePlayer;

public record UpdatePlayerCommand([FromRoute] Guid Id, string Name) : IRequest<OneOf<Player, NotFound>>;

public record UpdatePlayerResponse(Guid Id, string Name);

public class CreatePlayerValidator : Validator<UpdatePlayerCommand>
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

public static class UpdatePlayerMapper
{
    public static UpdatePlayerResponse ToResponse(this Player player)
    {
        return new UpdatePlayerResponse(player.Id, player.Name);
    }
}