using Chaos.Models.Menu;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public class LearnScorpionFormScript : MonkFormScriptBase
{
    public LearnScorpionFormScript(
        Dialog subject,
        IDialogFactory dialogFactory,
        ISkillFactory skillFactory,
        ISpellFactory spellFactory)
        : base(subject, dialogFactory, skillFactory, spellFactory) { }

    protected override MonkFormType FormType => MonkFormType.Kelberoth;
    protected override string StanceSpellKey => "scorpionStance";
    protected override string SkillKey => "poisonPunch";
    protected override byte RequiredStr => 18;
    protected override byte RequiredInt => 6;
    protected override byte RequiredWis => 8;
    protected override byte RequiredCon => 22;
    protected override byte RequiredDex => 11;
}