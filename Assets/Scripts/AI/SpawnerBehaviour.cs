using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : AIBehaviour
{
    [SerializeField]
    GameObject stdZombiePrefab;
    [SerializeField]
    AudioSource spawnSoundAudioSource;
    bool restNeeded = false;

    public override void Init(Unit myZombie)
    {
       
    }

    public override void Run(Unit myZombie)
    {
        //spawner only spawns a unit every two turns
        if (restNeeded)
        {
            restNeeded = false;
            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
        }
        else
        {
            
            restNeeded = true;
            //play spawning-soundeffect and Animation
            if(spawnSoundAudioSource!=null)
                spawnSoundAudioSource.Play();
            StartCoroutine(PlaySpawnAnimation(myZombie));
            //look for a free adjacent tile and select a random one
            Vector2Int[] adjacentSpaces = new Vector2Int[4];
            adjacentSpaces[0] = new Vector2Int(myZombie.gridPosition.x + 1, myZombie.gridPosition.y);
            adjacentSpaces[1] = new Vector2Int(myZombie.gridPosition.x - 1, myZombie.gridPosition.y);
            adjacentSpaces[2] = new Vector2Int(myZombie.gridPosition.x, myZombie.gridPosition.y + 1);
            adjacentSpaces[3] = new Vector2Int(myZombie.gridPosition.x, myZombie.gridPosition.y - 1);
            Vector2Int zombieSpawn = new Vector2Int(-1, -1);
            System.Random rnd = new System.Random();
            foreach (Vector2Int position in adjacentSpaces)
            {
                if (IsoGrid.instance.IsInsideBounds(position) && !MapContent.instance.Dictionary.ContainsKey(position))
                {
                    if (zombieSpawn == new Vector2Int(-1, -1))
                    {
                        zombieSpawn = position;
                    }
                    else
                    {
                        if (rnd.NextDouble() > 0.5d)
                        {
                            zombieSpawn = position;
                        }
                    }
                }
            }
            //if no free adjacent space was found, do nothing
            if (zombieSpawn == new Vector2Int(-1, -1))
            {
                return;
            }

            //create a new zombie at adjacent free space and add it to the MapContent and AiManager
            Unit newZombieUnit = GameObject.Instantiate(stdZombiePrefab, IsoGrid.instance.ToWorldSpace(zombieSpawn), this.transform.rotation).GetComponent<Unit>();
            newZombieUnit.RegisterThisUnit(zombieSpawn);
            newZombieUnit.MoveUnitTo(zombieSpawn);

            StartCoroutine(ZombieHelper.EndTurnAfterDelay());
        }
    }
    /// <summary>
    /// plays spawn animation
    /// </summary>
    /// <param name="myZombie"></param>
    /// <returns></returns>
    private IEnumerator PlaySpawnAnimation(Unit myZombie)
    {
        if (myZombie.MyAnimator != null)
        {
            myZombie.MyAnimator.SetBool("attacking", true);
            yield return new WaitForSeconds(myZombie.AttackDuration);
            myZombie.MyAnimator.SetBool("attacking", false);
        }
    }
}
