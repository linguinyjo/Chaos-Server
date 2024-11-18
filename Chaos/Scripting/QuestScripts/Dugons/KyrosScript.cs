using Chaos.Common.Definitions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.TrainingQuest;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons;

public class KyrosScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    
    /// <inheritdoc />
    public KyrosScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        // which option to show? 
        if (!source.HasClass(BaseClass.Monk)) return;
        
        var currentDugon = source.Trackers.Enums.TryGetValue<Dugon>(out var status) ? status : Dugon.None;

        if (currentDugon == Dugon.None) 
        {
            Subject.AddOption("White Dugon", "white_dugon_1");
            return;
        } else if (currentDugon == Dugon.White)
        {
            Subject.AddOption("Green Dugon", "green_dugon_1");
            return;
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null) {}
}
