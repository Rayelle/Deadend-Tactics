using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGeneration;

public class IsoGrid: MonoBehaviour
{
    public static IsoGrid instance;

    private tileType[,] map;

    private uint gridWidth=10, gridHeight=10;
    [SerializeField]
    float worldWidth, worldHeight;

    public uint GridHeight { get => gridHeight; }
    public uint GridWidth { get => gridWidth; }


    private void Awake()
    {
        //singleton
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    /// <summary>
    /// True if given space is inside current map-bounds, false if it is not.
    /// </summary>
    /// <param name="check"></param>
    /// <returns></returns>
    public bool IsInsideBounds(Vector2Int check)
    {
        if (check.x >= gridWidth || check.y >= gridHeight || check.x < 0|| check.y < 0)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Returns world-position for a given grid-position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 ToWorldSpace(uint x, uint y)
    {
        Vector3 inWorldSpace = new Vector3(0, 0, 0);
        inWorldSpace += new Vector3(x * worldWidth / 2, -x * worldHeight / 2, 0);
        inWorldSpace += new Vector3(y*worldWidth/2, y*worldHeight/2,0);

        return inWorldSpace;
    }
    /// <summary>
    /// Returns world coordinates for given grid position. Components of Vector2Int must be non-negative.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 ToWorldSpace(Vector2Int position)
    {
        Vector3 inWorldSpace = new Vector3(0, 0, 0);
        inWorldSpace += new Vector3(position.x * worldWidth / 2, -position.x * worldHeight / 2, 0);
        inWorldSpace += new Vector3(position.y*worldWidth/2, position.y*worldHeight/2,0);

        return inWorldSpace;
    }
    /// <summary>
    /// returns a tileType for given position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public tileType GetMapPoint(uint x, uint y)
    {
        return map[x, y];
    }
    /// <summary>
    /// Sets terrain-type for a given space.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="newTerrain"></param>
    public void SetMapPoint(uint x, uint y, tileType newTerrain)
    {
        map[x, y] = newTerrain;
    }
    /// <summary>
    /// Initialize map with width and height. Fill all spaces with dirt-terrain.
    /// </summary>
    /// <param name="newGridWidth"></param>
    /// <param name="newGridHeight"></param>
    public void InitIsoGrid(uint newGridWidth,uint newGridHeight)
    {
        gridWidth = newGridWidth;
        gridHeight = newGridHeight;
        map = new tileType[gridWidth, gridHeight];
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                map[j, i] = tileType.dirt;
            }
        }
    }
}

