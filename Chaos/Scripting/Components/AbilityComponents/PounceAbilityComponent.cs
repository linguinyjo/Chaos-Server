using Chaos.Extensions.Geometry;
using Chaos.Models.Data;
using Chaos.Models.World.Abstractions;
using Chaos.Scripting.Components.Abstractions;
using Chaos.Scripting.Components.Execution;

namespace Chaos.Scripting.Components.AbilityComponents;

public struct PounceAbilityComponent : IComponent
{
    /// <inheritdoc />
    public void Execute(ActivationContext context, ComponentVars vars)
    {
        var options = vars.GetOptions<IPounceComponentOptions>();
        var targets = vars.GetTargets<Creature>();

        if (options.WithAmbush == false)
        {
            Charge(targets, context);
        }
        else
        {
            Pounce(targets, context, options.Distance);
        }
    }

    private static void Charge(IReadOnlyCollection<Creature> targets, ActivationContext context)
    {
        foreach (var target in targets)
        {
            // Get the direction from the target to the player
            var directionToPlayer = target.DirectionalRelationTo(context.Source);

            // Reverse the direction to get the front position
            var frontOfTargetDirection = directionToPlayer.Reverse();
            // Get the point directly in front of the target, in the direction of the player
            var destinationPoint = target.DirectionalOffset(frontOfTargetDirection);

            // Check if the point is walkable and not blocked
            if (!context.TargetMap.IsWalkable(destinationPoint, context.Source) ||
                context.TargetMap.IsBlockingReactor(destinationPoint)) continue;

            context.Source.WarpTo(destinationPoint);
            context.Source.Turn(context.Source.Direction);
            return;
        }
    }

    private static void Pounce(IReadOnlyCollection<Creature> targets, ActivationContext context, int? optionsDistance)
    {
        foreach (var target in targets)
        {
            var endPoint = context.Source.DirectionalOffset(context.Source.Direction, optionsDistance ?? 3);

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
                        if (!context.TargetMap.IsWalkable(destinationPoint, context.Source)
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

    public interface IPounceComponentOptions
    {
        int? Distance { get; init; }
        bool? WithAmbush { get; init; }
    }
}