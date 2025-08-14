using Chaos.Common.Definitions;
using Chaos.Models.Abstractions;
using Chaos.Models.Menu;
using Chaos.Models.World;
using Chaos.Scripting.DialogScripts.Abstractions;
using Chaos.Scripting.QuestScripts.DarkThings;
using Chaos.Services.Factories;
using Chaos.Services.Factories.Abstractions;

namespace Chaos.Scripting.QuestScripts.FromTheHeart;

public class FaerieDialogScript:  DialogScriptBase
{
    private readonly IDialogFactory DialogFactory;
    private readonly Dialog Dialog;
    
    /// <inheritdoc />
    public FaerieDialogScript(Dialog subject, IDialogFactory dialogFactory)
        : base(subject)
    {
        DialogFactory = dialogFactory;
        Dialog = subject;
    }

    /// <inheritdoc />
    public override void OnDisplaying(Aisling source)
    {
        var faerieEntity = new FaerieDialogSource();
        Subject.DialogSource = faerieEntity;
    }
    
    public override void OnDisplayed(Aisling source) {}

    public override void OnNext(Aisling source, byte? optionIndex = null)
    {}
}

public sealed class FaerieDialogSource : IDialogSourceEntity
{
    public DisplayColor Color => DisplayColor.Default;
    public EntityType EntityType => EntityType.Creature;
    public uint Id => 1;
    public string Name => "Faerie";
    public ushort Sprite => 4;
    public void Activate(Aisling source)
    {}
}
