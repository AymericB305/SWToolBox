using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManageDefenses.UpdateDefense;

public record UpdateDefenseCommand(
    [FromRoute] Guid GuildId,
    [FromRoute] Guid Id,
    long MonsterLeadId,
    long Monster2Id,
    long Monster3Id,
    string Description)
    : IRequest<OneOf<Defense, NotFound, Existing>>;

public record UpdateDefenseResponse(
    Guid Id,
    MonsterResponse MonsterLead,
    MonsterResponse Monster2,
    MonsterResponse Monster3,
    string Description);
public record MonsterResponse(long Id, string Name);

public class UpdateDefenseValidator : Validator<UpdateDefenseCommand>
{
    public UpdateDefenseValidator()
    {
        RuleFor(x => x.MonsterLeadId)
            .NotEqual(x => x.Monster2Id)
            .WithMessage("2 monsters can't be in the same defense")
            .NotEqual(x => x.Monster3Id)
            .WithMessage("2 monsters can't be in the same defense");

        RuleFor(x => x.Monster2Id)
            .NotEqual(x => x.Monster3Id)
            .WithMessage("2 monsters can't be in the same defense");
    }
}

public static class UpdateDefenseMapper
{
    public static UpdateDefenseResponse ToResponse(this Defense defense)
    {
        return new UpdateDefenseResponse(
            defense.Id,
            defense.MonsterLead.ToResponse(),
            defense.Monster2.ToResponse(),
            defense.Monster3.ToResponse(),
            defense.Description
        );
    }

    private static MonsterResponse ToResponse(this Monster monster)
    {
        return new MonsterResponse(monster.Id, monster.Name);
    }
}