using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GoblinAlliance;

public class GoblinAllianceKillScript : AllianceKillScriptBase
{
    public GoblinAllianceKillScript(Monster subject, IItemFactory itemFactory)
        : base(subject, itemFactory)
    {}

    protected override bool ShouldGiveGemstone(Aisling aisling)
    {
        return GoblinAllianceQuestHelper.GetQuestStatus(aisling) 
               == GoblinAllianceQuestStatus.Started;
    }

    protected override void PlayQuestSound(Aisling aisling)
    {
        GoblinAllianceQuestHelper.PlayQuestSound(aisling);
    }
}
