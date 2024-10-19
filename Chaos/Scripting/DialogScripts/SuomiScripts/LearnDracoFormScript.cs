using Chaos.Models.Menu;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public class LearnDracoFormScript : MonkFormScriptBase
{
    public LearnDracoFormScript(
        Dialog subject,
        IDialogFactory dialogFactory,
        ISkillFactory skillFactory,
        ISpellFactory spellFactory)
        : base(subject, dialogFactory, skillFactory, spellFactory) { }

    protected override MonkFormType FormType => MonkFormType.Draco;
    protected override string StanceSpellKey => "dracoStance";
    protected override string SkillKey => "dracoTailKick";
    protected override byte RequiredStr => 12;
    protected override byte RequiredInt => 4;
    protected override byte RequiredWis => 8;
    protected override byte RequiredCon => 28;
    protected override byte RequiredDex => 8;
}
