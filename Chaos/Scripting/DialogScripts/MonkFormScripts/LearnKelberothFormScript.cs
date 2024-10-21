using Chaos.Models.Menu;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public class LearnKelberothFormScript : MonkFormScriptBase
{
    public LearnKelberothFormScript(
        Dialog subject,
        IDialogFactory dialogFactory,
        ISkillFactory skillFactory,
        ISpellFactory spellFactory)
        : base(subject, dialogFactory, skillFactory, spellFactory) { }

    protected override MonkFormType FormType => MonkFormType.Kelberoth;
    protected override string StanceSpellKey => "kelberothStance";
    protected override string SkillKey => "kelberothStrike";
    protected override byte RequiredStr => 30;
    protected override byte RequiredInt => 4;
    protected override byte RequiredWis => 4;
    protected override byte RequiredCon => 16;
    protected override byte RequiredDex => 6;
}