using Chaos.Collections;
using Chaos.Common.Definitions;
using Chaos.Models.Data;
using Chaos.Models.Legend;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.NLog.Logging.Definitions;
using Chaos.NLog.Logging.Extensions;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.DialogScripts.TrainerScripts;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;
using Chaos.Time;

namespace Chaos.Scripting.DialogScripts.ClassChangeScripts;

public class FirstClassChangeScript : ConfigurableDialogScriptBase
{
    private readonly ISimpleCache SimpleCache;
    private readonly ISkillFactory SkillFactory;

    
    #region ScriptVars
    protected byte Class { get; init; }
    #endregion

    private const byte SOUND = 42;

    private Animation Animation { get; } = new()
    {
        AnimationSpeed = 100,
        TargetAnimation = 50
    };

    /// <inheritdoc />
    public FirstClassChangeScript(Dialog subject, ISimpleCache simpleCache, ISkillFactory skillFactory)
        : base(subject)
    {
        SimpleCache = simpleCache;
        SkillFactory = skillFactory; 
    } 

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {}

    public override void OnDisplayed(Aisling source)
    {
        var baseClass = (BaseClass)Class;
        source.UserStatSheet.SetBaseClass(baseClass);
        switch (baseClass)
        {
            case BaseClass.Monk:
                AddClassSpecificAssail(source, "monkAssail");
                break;
            case BaseClass.Rogue:
                AddClassSpecificAssail(source, "rogueAssail");
                break;
        }

        var legendMark = new LegendMark(
            $"Became a {baseClass}",
            "choosingAClass",
            MarkIcon.Victory,
            MarkColor.White,
            1,
            GameTime.Now);
        source.Legend.AddUnique(legendMark);
        source.Animate(Animation);
        source.Client.SendSound(SOUND, false); 
    }

    private void AddClassSpecificAssail(Aisling source, string assail)
    {
        var isRemoved = source.SkillBook.TryGetRemove("assail", out var removedAssail);
        if (!isRemoved) return;
        var skill = SkillFactory.Create(assail);
        if (removedAssail != null) source.SkillBook.TryAdd(removedAssail.Slot, skill);
    }

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {
        var targetMap = SimpleCache.Get<MapInstance>("milethVillage");
        var destination = new Location("milethVillage", 94, 12);
        source.TraverseMap(targetMap, destination);
    }
}