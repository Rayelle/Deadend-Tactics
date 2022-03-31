using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterBehaviour : AIBehaviour
{
    [SerializeField]
    float movementAnimationWaitSeconds;
    private Vector2Int destination;
    private List<PathfindingNode> EachStepToDestination;
    [SerializeField]
    GameObject spitballUpPrefab, spitballDownPrefab;
    public override void Init(Unit myZombie)
    {


    }
    public override void Run(Unit myZombie)
    {
        //stunned zombies skip turn and deactivate stun
        if (myZombie.Stunned)
        {
            myZombie.Stunned = false;
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
            return;
        }

        //if this zombie is already in range, attack
        List<Unit> heroesInRange = ZombieHelper.HeroesInRange(myZombie.gridPosition, myZombie.AttackRange);
        if(heroesInRange.Count > 0)
        {
            //play attack animation and deal damage to target
            StartCoroutine(ZombieHelper.AnimateRangedAttack(myZombie,spitballUpPrefab,spitballDownPrefab));

            return;
        }

        List<PathfindingNode> allPaths = ZombieHelper.GetPathsInRange(myZombie);
        //if no spaces are reachable return without doing anything
        if (allPaths.Count == 0)
        {
            GameEvents.instance.UnitDoneMoving();
            return;
        }

        PathfindingNode bestPath = ZombieHelper.GetShortestPath(allPaths);

        //if path exceeds moverange, take fewer steps along the same path
        while (myZombie.MoveRange < bestPath.stepsToPathtaker)
        {
            bestPath = bestPath.predecessor;
        }

        //walk to position then play attack animation and deal damage to target
        StartCoroutine(ZombieHelper.AnimateWalkThenRangedAttack(bestPath.EachStepToNode(), myZombie,spitballUpPrefab,spitballDownPrefab));
    }

}
