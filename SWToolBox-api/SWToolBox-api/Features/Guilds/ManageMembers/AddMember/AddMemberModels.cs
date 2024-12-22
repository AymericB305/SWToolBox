using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManageMembers.AddMember;

public record AddMemberCommand([FromRoute] Guid GuildId, Guid? PlayerId, string Name)
    : IRequest<OneOf<Player, Existing, NotFound>>;

public record AddMemberResponse(Guid Id, string Name);

public class AddMemberValidator : Validator<AddMemberCommand>
{
    public AddMemberValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name of the player is required.")
            .Must(x => x.Length is > 5 and < 50)
            .WithMessage("The name of the player must be between 5 and 50 characters.");
    }
}

public static class AddMemberMapper
{
    public static Player ToEntity(this AddMemberCommand command)
    {
        return new Player
        {
            Name = command.Name,
        };
    }

    public static AddMemberResponse ToResponse(this Player player)
    {
        return new AddMemberResponse(player.Id, player.Name);
    }
}