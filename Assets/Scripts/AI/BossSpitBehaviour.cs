using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpitBehaviour : AIBehaviour
{
    [SerializeField]
    float spitAnimationDuration,spitSoundEffectDelay;
    [SerializeField]
    GameObject spitUpPrefab, spitDownPrefab;
    [SerializeField]
    AudioSource spitAudio;
    public override void Init(Unit myZombie)
    {

    }

    public override void Run(Unit myZombie)
    {
        //choose hero with least health
        Unit bestTarget = null;
        foreach (Unit hero in HeroManager.instance.AllHeroes)
        {
            if (hero.Health > 0)
            {
                if (bestTarget == null)
                {
                    bestTarget = hero;
                }
                else
                {
                    if (bestTarget.Health > hero.Health)
                    {
                        bestTarget = hero;
                    }
                }
            }
        }
        if (bestTarget == null)
        {
            //no targets found
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
            return;
        }
        
        StartCoroutine(SpitAtSpace(bestTarget.gridPosition,myZombie));
        //go back to boss-meleebehaviour
        myStateMachine.ChangeState();
    }
    /// <summary>
    /// Animate attack and create upwards-projectile at the right moment
    /// create downwards-projectile after delay and create acid spaces
    /// </summary>
    /// <param name="acidSpace"></param>
    /// <param name="myBoss"></param>
    /// <returns></returns>
    private IEnumerator SpitAtSpace(Vector2Int acidSpace,Unit myBoss)
    {
        //start animation
        if (myBoss.MyAnimator != null)
        {
            myBoss.MyAnimator.SetBool("spit", true);
        }
        yield return new WaitForSeconds(spitSoundEffectDelay);

        //after delay play sound effect and create upwards-projectile
        if (spitAudio != null)
            spitAudio.Play();

        GameObject.Instantiate(spitUpPrefab, myBoss.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(spitAnimationDuration-spitSoundEffectDelay);
        //end animation
        if (myBoss.MyAnimator != null)
        {
            myBoss.MyAnimator.SetBool("spit", false);
        }
        //create downwards projectile above target
        Vector3 targetWorldPosition = IsoGrid.instance.ToWorldSpace(acidSpace);
        targetWorldPosition.y += 10f;
        GameObject.Instantiate(spitDownPrefab, targetWorldPosition, Quaternion.identity)
        .GetComponentInChildren<SpitBallAnimation>().ShootDownwardsTowards(IsoGrid.instance.ToWorldSpace(acidSpace));

        yield return new WaitForSeconds(ZombieHelper.SpitballTravelTime);

        //when donwards-projectile hits the target, create acid-spaces at target and adjacent positions
        BossAcid.instance.AddAcidSpace(acidSpace);
 
        BossAcid.instance.AddAcidSpace(new Vector2Int(acidSpace.x + 1, acidSpace.y));
        BossAcid.instance.AddAcidSpace(new Vector2Int(acidSpace.x - 1, acidSpace.y));
        BossAcid.instance.AddAcidSpace(new Vector2Int(acidSpace.x, acidSpace.y + 1));
        BossAcid.instance.AddAcidSpace(new Vector2Int(acidSpace.x, acidSpace.y - 1));

        //end turn for boss
        GameEvents.instance.UnitDoneMoving();


    }
}
