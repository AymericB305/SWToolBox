using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SWToolBox_api.Common.Models;
using SWToolBox_api.Database.Entities;

namespace SWToolBox_api.Features.Guilds.ManageDefenses.CreateDefense;

public record CreateDefenseCommand(
    [FromRoute] Guid GuildId,
    long MonsterLeadId,
    long Monster2Id,
    long Monster3Id,
    string Description) : IRequest<OneOf<Defense, NotFound, Existing>>;

public record CreateDefenseResponse(
    Guid Id,
    MonsterResponse MonsterLead,
    MonsterResponse Monster2,
    MonsterResponse Monster3,
    string Description);
public record MonsterResponse(long Id, string Name);

public class CreateDefenseValidator : Validator<CreateDefenseCommand>
{
    public CreateDefenseValidator()
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

public static class CreateDefenseMapper
{
    public static Defense ToEntity(this CreateDefenseCommand command)
    {
        return new Defense
        {
            MonsterLeadId = command.MonsterLeadId,
            Monster2Id = command.Monster2Id,
            Monster3Id = command.Monster3Id,
            Description = command.Description,
        };
    }

    public static CreateDefenseResponse ToResponse(this Defense defense)
    {
        return new CreateDefenseResponse(
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