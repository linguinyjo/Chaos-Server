using Chaos.Formulae;
using Chaos.Objects.World;
using Chaos.Scripts.MonsterScripts.Abstractions;

namespace Chaos.Scripts.MonsterScripts.Components;

// ReSharper disable once ClassCanBeSealed.Global
public class DeathScript : MonsterScriptBase
{
    /// <inheritdoc />
    public DeathScript(Monster subject)
        : base(subject) { }

    /// <inheritdoc />
    public override void OnDeath()
    {
        if (!Map.RemoveObject(Subject))
            return;

        var rewardTarget = Subject.AggroList
                                  .OrderByDescending(kvp => kvp.Value)
                                  .Select(kvp => Map.TryGetObject<Aisling>(kvp.Key, out var a) ? a : null)
                                  .FirstOrDefault(a => a is not null);

        if (Subject.LootTable != null)
            Subject.Items.AddRange(Subject.LootTable.GenerateLoot());

        Subject.DropGold(Subject, Subject.Gold);
        Subject.Drop(Subject, Subject.Items);

        if (rewardTarget is not null)
        {
            var grp = rewardTarget.Group?.ToArray() ?? new[] { rewardTarget };
            var exp = ExpFormulae.Default.Calculate(Subject, grp);

            foreach (var member in grp)
                member.GiveExp(exp);
        }
    }
}