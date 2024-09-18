using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Templates.Abstractions;

namespace Chaos.Models.Templates;

public sealed record SpellTemplate : PanelEntityTemplateBase
{
    public required byte CastLines { get; init; }
    public required LearningRequirements? LearningRequirements { get; init; }
    public required bool LevelsUp { get; init; }
    public required byte MaxLevel { get; init; }
    public required string? Prompt { get; set; }
    public required SpellType SpellType { get; init; }
    public required SpellCategory SpellCategory { get; init; }
}