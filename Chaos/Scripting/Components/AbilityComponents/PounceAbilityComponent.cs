using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Extensions;
using Chaos.Extensions.Geometry;
using Chaos.Models.Data;
using Chaos.Models.World;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;
using Chaos.Scripting.FunctionalScripts.Abstractions;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct PounceAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        // var options = vars.GetOptions<IPounceComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        foreach (var target in targets)
        {
            var endPoint = context.Source.DirectionalOffset(context.Source.Direction, 3);

            var points = context.Source
                .GetDirectPath(endPoint)
                .Skip(1);

            foreach (var point in points)
            {
                if (context.TargetMap.IsWall(point) || context.TargetMap.IsBlockingReactor(point))
                    return;

                // var entity = context.TargetMap
                //     .GetEntitiesAtPoint<Creature>(point)
                //     .TopOrDefault();

                if (target != null)
                {
                    //get the direction that vectors behind the target relative to the source
                    var behindTargetDirection = target.DirectionalRelationTo(context.SourcePoint);

                    //for each direction around the target, starting with the direction behind the target
                    foreach (var direction in behindTargetDirection.AsEnumerable())
                    {
                        //get the point in that direction
                        var destinationPoint = target.DirectionalOffset(direction);

                        //if that point is not walkable or is a reactor, continue
                        if (!context.TargetMap.IsWalkable(destinationPoint, context.Source.Type)
                            || context.TargetMap.IsBlockingReactor(destinationPoint))
                            continue;

                        //if it is walkable, warp to that point and turn to face the target
                        context.Source.WarpTo(destinationPoint);
                        var newDirection = target.DirectionalRelationTo(context.Source);
                        context.Source.Turn(newDirection);

                        return;
                    }
                }
            }
        }
    }
}