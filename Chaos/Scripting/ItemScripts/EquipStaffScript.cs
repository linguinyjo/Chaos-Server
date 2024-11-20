using Chaos.Common.Definitions;
using Chaos.Models.Panel;
using Chaos.Models.World;
using Chaos.Scripting.ItemScripts.Abstractions;

namespace Chaos.Scripting.ItemScripts;

public class EquipStaffScript : ConfigurableItemScriptBase
{
    /// <inheritdoc />
    public EquipStaffScript(Item subject)
        : base(subject) { }

    /// <inheritdoc />
    public override void OnEquipped(Aisling aisling)
    {
        foreach (var spell in aisling.SpellBook)
            if (spell.Template.SpellCategory == SpellCategory)
            {
                aisling.SpellBook.Update(spell.Slot, lSpell => lSpell.CastLines = (byte)(lSpell.Template.CastLines - 1));
            }
    }

    /// <inheritdoc />
    public override void OnUnEquipped(Aisling aisling)
    {
        foreach (var spell in aisling.SpellBook)
            if (spell.Template.SpellCategory == SpellCategory)
            {
                aisling.SpellBook.Update(spell.Slot, lSpell => lSpell.CastLines = (byte)2);
            }
    }
    

    #region ScriptVars
    public SpellCategory? SpellCategory { get; init; }
    #endregion
}