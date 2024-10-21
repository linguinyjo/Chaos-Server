using Chaos.Models.Menu;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public class LearnWhiteBatFormScript : MonkFormScriptBase
{
    public LearnWhiteBatFormScript(
        Dialog subject,
        IDialogFactory dialogFactory,
        ISkillFactory skillFactory,
        ISpellFactory spellFactory)
        : base(subject, dialogFactory, skillFactory, spellFactory) { }

    protected override MonkFormType FormType => MonkFormType.WhiteBat;
    protected override string StanceSpellKey => "whiteBatStance";
    protected override string SkillKey => "darkSpear";
    protected override byte RequiredStr => 12;
    protected override byte RequiredInt => 10;
    protected override byte RequiredWis => 8;
    protected override byte RequiredCon => 4;
    protected override byte RequiredDex => 22;
}