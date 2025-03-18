using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.Dugons.BlueDugonScripts;

public class StartBlueDugonScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly BlueDugonQuestHelper BlueDugonQuestHelper = new();

    /// <inheritdoc />
    public StartBlueDugonScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject) => DialogFactory = dialogFactory;

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        if (!BlueDugonQuestHelper.IsEligible(source)) return;
        
        var questStarted = BlueDugonQuestHelper.StartQuest(source);
        if (questStarted)
        {
            Subject.Reply(
                source, 
                "Excellent. Let's begin. To obtain the Blue Dugon, you must prove a basic level of combat proficiency.",
                "blue_dugon_start_1"
            );
        }
        else
        {
            Subject.Reply(
                source, 
                "You need to spend more time reflecting on your previous failure. Once you have done this, I will allow you to try the test once more.",
                "close"
            );
        }
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {}
}
