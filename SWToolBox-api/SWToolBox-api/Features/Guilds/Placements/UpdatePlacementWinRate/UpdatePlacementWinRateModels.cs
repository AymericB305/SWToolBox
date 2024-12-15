using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace SWToolBox_api.Features.Guilds.Placements.UpdatePlacementWinRate;

public record UpdatePlacementWinRateCommand([FromRoute] Guid Id, short Wins, short Losses)
    : IRequest<OneOf<Success, NotFound>>;
    
public class CreateGuildValidator : Validator<UpdatePlacementWinRateCommand>
{
    public CreateGuildValidator()
    {
        RuleFor(x => x.Wins)
            .GreaterThan<UpdatePlacementWinRateCommand, short>(-1)
            .WithMessage("The number of wins must be positive");
        
        RuleFor(x => x.Losses)
            .GreaterThan<UpdatePlacementWinRateCommand, short>(-1)
            .WithMessage("The number of losses must be positive");
    }
}