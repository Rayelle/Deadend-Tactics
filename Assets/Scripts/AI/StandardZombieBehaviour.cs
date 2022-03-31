using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardZombieBehaviour : AIBehaviour
{
    [SerializeField]
    float movementAnimationWaitSeconds;

    //private Vector2Int destination;
    //private List<PathfindingNode> EachStepToDestination;
    public override void Init(Unit myZombie)
    {
        
    }
    public override void Run(Unit myZombie)
    {
        //if unit is stunned, clear stun and skip turn
        if (myZombie.Stunned)
        {
            myZombie.Stunned = false;
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
            return;
        }

        //if this zombie is already in melee range
        List<Unit> heroesInRange = ZombieHelper.HeroesInMeleeRange(myZombie.gridPosition);

        if (heroesInRange.Count > 0)
        {
            Unit bestHeroToAttack = ZombieHelper.GetBestTarget(heroesInRange);

            //play attack animation and deal damage to target
            StartCoroutine(ZombieHelper.AnimateMeleeAttack(myZombie));

            return;
        }

        //check for possible hero attack locations
        List<PathfindingNode> allPaths = ZombieHelper.GetPathsInMeleeRange(myZombie.gridPosition);

        //if no spaces are reachable end turn
        if (allPaths.Count == 0)
        {
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
            return;
        }

        //look for the shortest path
        PathfindingNode bestPath = ZombieHelper.GetShortestPath(allPaths);

        //if path exceeds moverange, take fewer steps along the same path
        while (myZombie.MoveRange < bestPath.stepsToPathtaker)
        {
            bestPath = bestPath.predecessor;
        }

        //walk to position then play attack animation and deal damage to target
        StartCoroutine(ZombieHelper.AnimateWalkThenMeleeAttack(bestPath.EachStepToNode(),myZombie));
    }
}
