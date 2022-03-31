using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberBehaviour : AIBehaviour
{
    [SerializeField]
    float movementAnimationWaitSeconds;

    //private Vector2Int destination;
    //private List<PathfindingNode> EachStepToDestination;
    private Unit currentlyGrabbedHero = null;
    bool isGrabbingSomeone = false;

    public override void Init(Unit myZombie)
    {
        
    }
    private void OnDestroy()
    {
        //reset herostun on death
        if(currentlyGrabbedHero!=null)
            currentlyGrabbedHero.Stunned = false;
    }
    public override void Run(Unit myZombie)
    {
        //if a hero is grabbed, this zombie delas damage to grabbed hero
        if (currentlyGrabbedHero!=null)
        {
            currentlyGrabbedHero.TakeDamage(myZombie.GetAttackDamage());
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

        //if this zombie is already in melee range, attack
        List<Unit> heroesInRange = ZombieHelper.HeroesInMeleeRange(myZombie.gridPosition);

        if (heroesInRange.Count > 0)
        {
            //get best target and grab it, play attack-animation
            Unit bestHeroToAttack = ZombieHelper.GetBestTarget(heroesInRange);

            GrabHero(bestHeroToAttack);

            StartCoroutine(ZombieHelper.AnimateMeleeAttack(myZombie));

            return;
        }


        List<PathfindingNode> allPaths = ZombieHelper.GetPathsInMeleeRange(myZombie.gridPosition);


        //if no spaces are reachable return without doing anything
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

        //walk to position then play attack animation, grab and deal damage to target
        StartCoroutine(AnimateWalkThenGrab(bestPath.EachStepToNode(),myZombie));
    }
    /// <summary>
    /// Animates walking to target location, then if target is adjacent animate attack and grab target
    /// </summary>
    /// <param name="eachStepToDestination"></param>
    /// <param name="myZombie"></param>
    /// <returns></returns>
    IEnumerator AnimateWalkThenGrab(List<PathfindingNode> eachStepToDestination, Unit myZombie)
    {
        //start walk-animation
        if (myZombie.MyAnimator != null)
        {
            myZombie.MyAnimator.SetBool("walking", true);
        }
        //move unit along path with a short delay between each step
        foreach (PathfindingNode current in eachStepToDestination)
        {
            
            if (current.Position == eachStepToDestination[eachStepToDestination.Count - 1].Position)
            {
                myZombie.MoveUnitTo(current.Position);
                break;
            }
            myZombie.MoveUnitTo(current.Position);

            yield return new WaitForSeconds(ZombieHelper.MovementAnimationDelay);
        }
        //stop walk-animation
        if (myZombie.MyAnimator != null)
        {
            myZombie.MyAnimator.SetBool("walking", false);
        }


        //start attack
        List<Unit> heroesInRange = ZombieHelper.HeroesInMeleeRange(myZombie.gridPosition);

        if (heroesInRange.Count > 0)
        {
            Unit bestHeroToAttack = ZombieHelper.GetBestTarget(heroesInRange);

            //start attack-animation
            if (myZombie.MyAnimator != null)
            {
                myZombie.MyAnimator.SetBool("attacking", true);
            }
            yield return new WaitForSeconds(myZombie.AttackSoundEffectDelay);
            //play attack-soundeffect
            if (myZombie.AttackAudio != null)
            {
                myZombie.AttackAudio.Play();
            }
            GrabHero(bestHeroToAttack);
            bestHeroToAttack.TakeDamage(myZombie.GetAttackDamage());

            yield return new WaitForSeconds(myZombie.AttackDuration - myZombie.AttackSoundEffectDelay);
            //stop attack-animation
            if (myZombie.MyAnimator != null)
            {
                myZombie.MyAnimator.SetBool("attacking", false);
            }


        }

        GameEvents.instance.UnitDoneMoving();
    }
    /// <summary>
    /// sets up a hero to be stunned until the grabber dies
    /// </summary>
    /// <param name="hero"></param>
    private void GrabHero(Unit hero)
    {
        hero.Stunned = true;
        currentlyGrabbedHero = hero;
    }
}
