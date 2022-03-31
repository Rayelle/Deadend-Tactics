using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeBehaviour : AIBehaviour
{

    [SerializeField]
    BossSpitBehaviour mySpitBehaviour;
    public override void Init(Unit myZombie)
    {
        
    }

    public override void Run(Unit myZombie)
    {

        //if this zombie is already in melee range, attack
        List<Unit> heroesInRange = ZombieHelper.HeroesInMeleeRange(myZombie.gridPosition);

        if (heroesInRange.Count > 0)
        {
            //standing next to possible target, attack it
            Unit bestHeroToAttack = ZombieHelper.GetBestTarget(heroesInRange);

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

        while (myZombie.MoveRange < bestPath.stepsToPathtaker)
        {
            bestPath = bestPath.predecessor;
        }

        StartCoroutine(ZombieHelper.AnimateWalkThenMeleeAttack(bestPath.EachStepToNode(), myZombie));

        //change to boss-spitbehaviour for next activation
        myStateMachine.ChangeState(mySpitBehaviour);
    }
}
