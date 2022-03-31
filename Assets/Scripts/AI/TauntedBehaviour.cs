using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this behaviour is used by all zombies who are affected by the tanks taunt after receiving her taunt upgrade
public class TauntedBehaviour : AIBehaviour
{
    public static TauntedBehaviour instance;

    [SerializeField]
    private float movementAnimationWaitSeconds;
    private Vector2Int[] tankAdjacentSpaces = new Vector2Int[4];
    [SerializeField]
    GameObject upSpitPrefab, downSpitPrefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public override void Init(Unit myZombie)
    {
        
    }
    public override void Run(Unit myZombie)
    {
        //reset behaviour of zombie for next activation
        EndTauntedBehaviour();

        //if tank is dead, do nothing
        if (HeroManager.instance.Tank.Health <= 0)
        {
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
            return;
        }
        //stunned zombies skip turn and deactivate stun
        if (myZombie.Stunned)
        {
            myZombie.Stunned = false;
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
            return;
        }
        //if taunted unit is a spawner, do nothing
        SpawnerBehaviour mySpawner;
        if(myZombie.TryGetComponent<SpawnerBehaviour>(out mySpawner))
        {
            return;
        }
        //if taunted unit is a spitter set up isRanged for attack
        SpitterBehaviour mySpitter;
        bool isRanged=false;
        if (myZombie.TryGetComponent<SpitterBehaviour>(out mySpitter))
        {
            isRanged = true;
        }

        //if already next to tank, play attack animation and deal damage
        if (CheckTankAdjacent(myZombie))
        {
            if (isRanged)
            {
                StartCoroutine(ZombieHelper.AnimateRangedAttack(myZombie, upSpitPrefab, downSpitPrefab));

            }
            else
            {
                StartCoroutine(ZombieHelper.AnimateMeleeAttack(myZombie));
            }
            return;
        }

        PathfindingNode bestPath = null;
        //look for a path to a tile which is adjacent to the tank
        foreach (Vector2Int tankAdjacentSpace in tankAdjacentSpaces)
        {

            PathfindingNode possiblePath = Pathfinding.FindPath(myZombie.gridPosition, tankAdjacentSpace);
            if (possiblePath != null)
            {
                if (bestPath == null)
                {
                    bestPath = possiblePath;
                }
                else
                {
                    if (bestPath.stepsToPathtaker > possiblePath.stepsToPathtaker)
                    {
                        bestPath = possiblePath;
                    }
                }
            }
        }
        //if all spaces adjacent to tank are unreachable, do nothing
        if (bestPath == null)
        {
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
            return;
        }

        //if path exceeds moverange, take fewer steps along the same path
        while (bestPath.stepsToPathtaker > myZombie.MoveRange)
        {
            bestPath = bestPath.predecessor;
        }

        //walk to position then play attack animation and deal damage to target
        if (isRanged)
        {
            StartCoroutine(ZombieHelper.AnimateWalkThenRangedAttack(bestPath.EachStepToNode(), myZombie,upSpitPrefab,downSpitPrefab));
        }
        else
        {
            StartCoroutine(ZombieHelper.AnimateWalkThenMeleeAttack(bestPath.EachStepToNode(),myZombie));
        }

    }
    /// <summary>
    /// Check to see if the tank is at an adjacent space
    /// </summary>
    /// <param name="myZombie"></param>
    /// <returns></returns>
    private bool CheckTankAdjacent(Unit myZombie)
    {
        tankAdjacentSpaces[0] = new Vector2Int(HeroManager.instance.Tank.gridPosition.x + 1, HeroManager.instance.Tank.gridPosition.y);
        tankAdjacentSpaces[1] = new Vector2Int(HeroManager.instance.Tank.gridPosition.x - 1, HeroManager.instance.Tank.gridPosition.y);
        tankAdjacentSpaces[2] = new Vector2Int(HeroManager.instance.Tank.gridPosition.x, HeroManager.instance.Tank.gridPosition.y + 1);
        tankAdjacentSpaces[3] = new Vector2Int(HeroManager.instance.Tank.gridPosition.x, HeroManager.instance.Tank.gridPosition.y - 1);
        foreach (Vector2Int adjacentSpace in tankAdjacentSpaces)
        {
            if (myZombie.gridPosition == adjacentSpace)
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// switch to default behaviour for next activation
    /// </summary>
    private void EndTauntedBehaviour()
    {
        myStateMachine.ChangeState();
    }
}
