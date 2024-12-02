using Chaos.Collections;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.MonsterScripts.Abstractions;
using Chaos.Scripting.QuestScripts.Terror;
using Chaos.Services.Factories.Abstractions;
using Chaos.Storage.Abstractions;

namespace Chaos.Scripting.MonsterScripts;

public class SpawnHiddenScript : MonsterScriptBase
{
    private readonly ISimpleCache SimpleCache;
    private readonly IEffectFactory EffectFactory;
    
    /// <inheritdoc />
    public SpawnHiddenScript(Monster subject, ISimpleCache simpleCache, IEffectFactory effectFactory)
        : base(subject)
    {
        SimpleCache = simpleCache;
        EffectFactory = effectFactory;
    }

    /// <inheritdoc />
    public override void OnDeath()
    {
      
    }

    public override void OnSpawn()
    {
        var effect = EffectFactory.Create("hide");
        Subject.Effects.Apply(Subject, effect);
        base.OnSpawn();
    }

    /// <inheritdoc />
    public override void OnAttacked(Creature source, int damage)
    {
    } 
}
