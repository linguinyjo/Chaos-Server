using Chaos.Definitions;
using Chaos.Extensions;
using Chaos.Extensions.Common;
using Chaos.Extensions.Geometry;
using Chaos.Geometry.Abstractions;
using Chaos.Geometry.Abstractions.Definitions;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct GetTargetsAbilityComponent<TEntity> : IConditionalComponent where TEntity: MapEntity
{
    /// <inheritdoc />
    public bool Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IGetTargetsComponentOptions>();
        var direction = context.TargetCreature?.Direction ?? context.Target.DirectionalRelationTo(context.Source);
        var map = context.TargetMap;
        
        if (direction == Direction.Invalid)
            direction = context.Source.Direction;

        var targetPoints = options.Shape
                                  .ResolvePoints(
                                      context.TargetPoint,
                                      options.Range,
                                      direction,
                                      null,
                                      options.ExcludeSourcePoint)
                                  .ToListCast<IPoint>();

        var targetEntities = map.GetEntitiesAtPoints<TEntity>(targetPoints)
                                .WithFilter(context.Source, options.Filter)
                                .ToList();

        if (options.SingleTarget && (targetEntities.Count > 1))
        {
            if (context.TargetCreature is TEntity entity && targetEntities.Contains(entity))
                targetEntities = [entity];
            else
                targetEntities =
                [
                    targetEntities.OrderBy(e => e.Creation)
                                  .First()
                ];
        }

        vars.SetPoints(targetPoints);
        vars.SetTargets(targetEntities);

        return !options.MustHaveTargets || (targetEntities.Count != 0);
    }

    public interface IGetTargetsComponentOptions
    {
        bool ExcludeSourcePoint { get; init; }
        TargetFilter Filter { get; init; }
        bool MustHaveTargets { get; init; }
        int Range { get; init; }
        AoeShape Shape { get; init; }
        bool SingleTarget { get; init; }
    }
}