using Chaos.Data;
using Chaos.Schemas.Data;
using Chaos.TypeMapper.Abstractions;

namespace Chaos.MapperProfiles;

public class LearningRequirementMapperProfile : IMapperProfile<LearningRequirements, LearningRequirementsSchema>
{
    private readonly ITypeMapper Mapper;

    public LearningRequirementMapperProfile(ITypeMapper mapper) => Mapper = mapper;

    /// <inheritdoc />
    public LearningRequirements Map(LearningRequirementsSchema obj) => new()
    {
        RequiredStats = obj.RequiredStats == null ? null : Mapper.Map<Stats>(obj.RequiredStats),
        PrerequisiteSkillTemplateKeys = obj.PrerequisiteSkillTemplateKeys,
        PrerequisiteSpellTemplateKeys = obj.PrerequisiteSpellTemplateKeys,
        ItemRequirements = Mapper.MapMany<ItemRequirement>(obj.ItemRequirements).ToList(),
        RequiredGold = obj.RequiredGold,
        RequiredLevel = obj.RequiredLevel,
        RequiredClass = obj.RequiredClass
    };

    /// <inheritdoc />
    public LearningRequirementsSchema Map(LearningRequirements obj) => throw new NotImplementedException();
}