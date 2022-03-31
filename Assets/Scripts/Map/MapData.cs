using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGeneration;

//maybe change into scriptable object

public class MapData : MonoBehaviour
{
    [SerializeField]
    uint gridWidth, gridHeight;
    [SerializeField]
    HeroSpawnOpportunity[] possibleHeroSpawnPositions;

    [SerializeField]
    List<Vector2Int> zombieSpawnOpportunities;
    [SerializeField]
    Vector2Int[] allObstaclePositions;
    Vector2Int[] myHeroSpawn;

    HashSet<Vector2Int> usedSpawns = new HashSet<Vector2Int>();
    HashSet<Vector2Int> currentHeroSafeZone = null;
    System.Random rnd;

    

    public Vector2Int[] AllObstaclePositions { get => allObstaclePositions; }
    public uint GridWidth { get => gridWidth; }
    public uint GridHeight { get => gridHeight; }
    private void Start()
    {
        rnd = new System.Random((int)(Mathf.Pow((float)Time.time,(float)Time.time)));
    }
    /// <summary>
    /// gets position for creating a hero based on heroSpawnPositions
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetHeroSpawn()
    {
        return possibleHeroSpawnPositions[rnd.Next(possibleHeroSpawnPositions.Length)].HeroSpawnPositions;
    }
    /// <summary>
    /// gets one zombie spawn from the list of possible zombie spawns
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetOneZombieSpawn()
    {

        if (zombieSpawnOpportunities.Count == 0)
        {
            return new Vector2Int(-1, -1);
        }
        int i = rnd.Next(zombieSpawnOpportunities.Count);
        Vector2Int zombieSpawn = zombieSpawnOpportunities[i];
        zombieSpawnOpportunities.RemoveAt(i);
        return zombieSpawn;
    }
 
}
