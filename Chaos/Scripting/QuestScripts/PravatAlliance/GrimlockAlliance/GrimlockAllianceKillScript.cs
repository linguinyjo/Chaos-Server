using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.PravatAlliance.GrimlockAlliance;

/// <summary>
/// Specific implementation for Grimlock Alliance Kill Script
/// </summary>
public class GrimlockAllianceKillScript : AllianceKillScriptBase
{
    public GrimlockAllianceKillScript(Monster subject, IItemFactory itemFactory)
        : base(subject, itemFactory)
    {}

    protected override bool ShouldGiveGemstone(Aisling aisling)
    {
        return GrimlockAllianceQuestHelper.GetQuestStatus(aisling) 
               == GrimlockAllianceQuestStatus.Started;
    }

    protected override void PlayQuestSound(Aisling aisling)
    {
        GrimlockAllianceQuestHelper.PlayQuestSound(aisling);
    }
}
