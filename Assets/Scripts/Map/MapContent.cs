using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContent : MonoBehaviour
{
    public static MapContent instance;

    [SerializeField]
    MapGenerator myMapGenerator;
    
    Dictionary<Vector2Int, Unit> dictionary = new Dictionary<Vector2Int, Unit>();

    public Dictionary<Vector2Int, Unit> Dictionary { get => dictionary; }


    private void Start()
    {
        GameEvents.instance.onSpawnUnit += SpawnUnit;
        GameEvents.instance.onDestroyUnit += DespawnUnit;
    }

    private void OnDisable()
    {
        GameEvents.instance.onSpawnUnit -= SpawnUnit;
        GameEvents.instance.onDestroyUnit -= DespawnUnit;

    }

    /// <summary>
    /// Register a newly spawned unit inside of map-dictionary.
    /// </summary>
    /// <param name="newUnit"></param>
    public void SpawnUnit(Unit newUnit)
    {
        if (!dictionary.ContainsKey(newUnit.gridPosition))
        {
            AddUnit(newUnit.gridPosition, newUnit);
        }
    }
    /// <summary>
    /// delete a unit inside the map
    /// </summary>
    /// <param name="despawnedUnit"></param>
    public void DespawnUnit(Unit despawnedUnit)
    {
        if (despawnedUnit.MyUnitType == HeroEnums.UnitType.enemy)
        {
            DeleteAt(despawnedUnit.gridPosition);
        }
    }

    private void Awake()
    {
        //singleton
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    /// <summary>
    /// Add a given unit to the map dictionary at given position.
    /// </summary>
    /// <param name="startPoint">new unit position</param>
    /// <param name="adding">new unit</param>
    public void AddUnit(Vector2Int startPoint, Unit adding)
    {
        if( startPoint.x>=0&&startPoint.x<myMapGenerator.GridWidth&&
            startPoint.y >= 0 && startPoint.y < myMapGenerator.GridHeight)
        {
            if (!dictionary.ContainsValue(adding))
            {
                dictionary.Add( startPoint, adding);
            }
        }
    }
    public void DeleteAt(Vector2Int position)
    {
        if(dictionary.ContainsKey(position))
            dictionary.Remove(position);
    }
    /// <summary>
    /// Move a unit at given space towards a new space inside map-dictionary.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void MoveFromTo(Vector2Int from, Vector2Int to)
    {
        if (dictionary.ContainsKey(from))
        {
            if (!dictionary.ContainsKey(to))
            {
                if (to.x >= 0 && to.x < myMapGenerator.GridWidth &&
                    to.y >= 0 && to.y < myMapGenerator.GridHeight)
                {
                    Unit toMove = dictionary[from];
                    dictionary.Remove(from);
                    dictionary.Add(to, toMove);
                }
            }
        }
    }
    /// <summary>
    /// returns true if the given position contains a hero
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool SpaceContainsHero(Vector2Int position)
    {
        if (dictionary.ContainsKey(position))
        {
            if(dictionary[position].MyUnitType==HeroEnums.UnitType.gunner || dictionary[position].MyUnitType == HeroEnums.UnitType.tank || dictionary[position].MyUnitType == HeroEnums.UnitType.medic)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// returns true if the given position contains an obstacle
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool SpaceContainsObstacle(Vector2Int position)
    {
        if (dictionary.ContainsKey(position))
        {
            if(dictionary[position].MyUnitType==HeroEnums.UnitType.obstacle)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// returns true if the given position contains an enemy
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool SpaceContainsEnemy(Vector2Int position)
    {
        if (dictionary.ContainsKey(position))
        {
            if (dictionary[position].MyUnitType == HeroEnums.UnitType.enemy)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// try to get an enemy at given position
    /// if there is no enemy at position returns false
    /// </summary>
    /// <param name="position"></param>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public bool tryGetEnemy(Vector2Int position, out Unit enemy)
    {
        if (dictionary.ContainsKey(position))
        {
            if (dictionary[position].MyUnitType == HeroEnums.UnitType.enemy)
            {
                enemy = dictionary[position];
                return true;
            }
        }
        enemy = null;
        return false;
    }
    /// <summary>
    /// returns true if the given position contains nothing and is inside the map
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool SpaceIsFree(Vector2Int position)
    {
        if (IsoGrid.instance.IsInsideBounds(position))
        {
            if (!dictionary.ContainsKey(position))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// get all grid locations of spaces inside a certain range and inside the map-bounds.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public HashSet<Vector2Int> getSpacesInRange(Vector2Int position, int range)
    {
        HashSet<Vector2Int> spacesInRange = new HashSet<Vector2Int>();
        
        spacesInRange.Add(position);
        for (int i = 0; i < range; i++)
        {
            List<Vector2Int> toAdd = new List<Vector2Int>();
            foreach (Vector2Int space in spacesInRange)
            {
                Vector2Int[] adjacentSpaces = {
                    new Vector2Int(space.x + 1, space.y),
                    new Vector2Int(space.x - 1, space.y),
                    new Vector2Int(space.x , space.y + 1),
                    new Vector2Int(space.x , space.y - 1)};

                foreach (Vector2Int adjacentSpace in adjacentSpaces)
                {
                    if (IsoGrid.instance.IsInsideBounds(adjacentSpace))
                        toAdd.Add(adjacentSpace);
                }
            }
            foreach (Vector2Int spaceToAdd in toAdd)
            {
                spacesInRange.Add(spaceToAdd);
            }
        }

        return spacesInRange;
    }
}
