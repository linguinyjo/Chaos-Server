using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.DialogScripts.suomiScripts;

public abstract class MonkFormScriptBase : DialogScriptBase
{
    protected readonly IDialogFactory DialogFactory;
    protected readonly Dialog Dialog;
    protected readonly ISkillFactory SkillFactory;
    protected readonly ISpellFactory SpellFactory;
    private const int GoldRequiremnent = 50000;
    
    protected MonkFormScriptBase(
        Dialog subject,
        IDialogFactory dialogFactory,
        ISkillFactory skillFactory,
        ISpellFactory spellFactory) : base(subject)
    {
        Dialog = subject;
        DialogFactory = dialogFactory;
        SkillFactory = skillFactory;
        SpellFactory = spellFactory;
    }

    public override void OnDisplaying(Aisling source)
    {
        if (!CheckStatRequirements(source))
        {
            ShowMissingRequirementsDialog(source);
            return;
        }

        if (source.Gold < GoldRequiremnent)
        {
            ShowNotEnoughGoldDialog(source);
            return;
        }

        if (!TryLearnForm(source))
        {
            ShowSpellBookFullDialog(source);
            return;
        }

        ShowSuccessDialog(source);
    }

    private bool CheckStatRequirements(Aisling source)
    {
        return source.StatSheet.EffectiveStr >= RequiredStr &&
               source.StatSheet.EffectiveInt >= RequiredInt &&
               source.StatSheet.EffectiveWis >= RequiredWis &&
               source.StatSheet.EffectiveCon >= RequiredCon &&
               source.StatSheet.EffectiveDex >= RequiredDex;
    }

    private bool TryLearnForm(Aisling source)
    {
        var stance = SpellFactory.Create(StanceSpellKey);
        if (!source.SpellBook.TryAddToNextSlot(stance))
            return false;

        var skill = SkillFactory.Create(SkillKey);
        if (!source.SkillBook.TryAddToNextSlot(skill))
        {
            source.SpellBook.RemoveByTemplateKey(StanceSpellKey);
            return false;
        }

        var goldTaken = source.TryTakeGold(GoldRequiremnent);
        if (!goldTaken)
        {
            source.SpellBook.RemoveByTemplateKey(StanceSpellKey);
            source.SkillBook.RemoveByTemplateKey(StanceSpellKey);
            return false;
        }
        source.Trackers.Enums.Set(FormType);
        return true;
    }

    private void ShowMissingRequirementsDialog(Aisling source)
    {
        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            $"You do not meet the requirements to learn {FormType} Form. Please come back when you have grown stronger.")
        {
            NextDialogKey = "Close"
        };
        dialog.Display(source);
    }

    private void ShowNotEnoughGoldDialog(Aisling source)
    {
        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            $"Hold on, you think I'm running a charity here? Come back when you have the full {GoldRequiremnent}.")
        {
            NextDialogKey = "Close"
        };
        dialog.Display(source);
    }
    
    private void ShowSpellBookFullDialog(Aisling source)
    {
        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            "Please ensure you have room in your skillbook")
        {
            NextDialogKey = "Close"
        };
        dialog.Display(source);
    }

    private void ShowSuccessDialog(Aisling source)
    {
        var dialog = new Dialog(
            Dialog.DialogSource,
            DialogFactory,
            ChaosDialogType.Normal,
            $"Congratulations, you have learned {FormType} Form!")
        {
            NextDialogKey = "Close"
        };
        dialog.Display(source);
    }

    public override void OnDisplayed(Aisling source) { }
    public override void OnNext(Aisling source, byte? optionIndex = null) { }

    protected abstract MonkFormType FormType { get; }
    protected abstract string StanceSpellKey { get; }
    protected abstract string SkillKey { get; }
    protected abstract byte RequiredStr { get; }
    protected abstract byte RequiredInt { get; }
    protected abstract byte RequiredWis { get; }
    protected abstract byte RequiredCon { get; }
    protected abstract byte RequiredDex { get; }
}

public enum MonkFormType
{
    None = 0,
    Draco = 1,
    Kelberoth = 2,
    WhiteBat = 3,
    Scorpion = 4
}
