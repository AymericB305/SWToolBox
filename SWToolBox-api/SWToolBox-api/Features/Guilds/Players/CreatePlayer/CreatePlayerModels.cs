﻿using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.Players.CreatePlayer;

public record CreatePlayerCommand([FromRoute] Guid GuildId, string Name) : IRequest<OneOf<Player, Existing>>;
public record CreatePlayerResponse(Guid Id, string Name);

public class CreatePlayerValidator : Validator<CreatePlayerCommand>
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
    public static Player ToEntity(this CreatePlayerCommand command)
    {
        return new Player
        {
            Name = command.Name,
        };
    }

    public static CreatePlayerResponse ToResponse(this Player player)
    {
        return new CreatePlayerResponse(player.Id, player.Name);
    }
}