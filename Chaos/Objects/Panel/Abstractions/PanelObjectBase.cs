using Chaos.Core.Identity;
using Chaos.Scripts.Interfaces;
using Chaos.Templates.Abstractions;
using Chaos.Time.Interfaces;

namespace Chaos.Objects.Panel.Abstractions;

/// <summary>
///     Represents an object that exists within the in-game panels.
/// </summary>
public abstract class PanelObjectBase : IDeltaUpdatable, IScripted
{
    public TimeSpan Cooldown { get; set; }
    public TimeSpan? Elapsed { get; set; }
    public byte Slot { get; set; }
    public ISet<string> ScriptKeys { get; }
    public virtual PanelObjectTemplateBase Template { get; }
    public ulong UniqueId { get; }

    protected PanelObjectBase(PanelObjectTemplateBase template, ulong? uniqueId = null, int? elapsedMs = null)
    {
        uniqueId ??= ServerId.NextId;
        Template = template;
        Cooldown = template.Cooldown;
        ScriptKeys = new HashSet<string>(template.ScriptKeys, StringComparer.OrdinalIgnoreCase);
        UniqueId = uniqueId.Value;

        if (elapsedMs.HasValue)
            Elapsed = TimeSpan.FromMilliseconds(elapsedMs.Value);
    }

    public virtual bool CanUse() => (Cooldown.TotalMilliseconds == 0) || !Elapsed.HasValue || (Elapsed > Cooldown);

    public override string ToString() => $@"(Id: {UniqueId}, Name: {Template.Name})";

    public void Update(TimeSpan delta)
    {
        if (Elapsed.HasValue)
        {
            var value = Elapsed.Value + delta;
            Elapsed = value;
        }
    }
}