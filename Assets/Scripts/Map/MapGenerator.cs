using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGeneration;

namespace MapGeneration
{

    public enum tileType
    {
        grass,
        dirt,
        concrete
    }
}

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;
    [SerializeField]
    uint gridWidth, gridHeight;
    [SerializeField]
    float worldWidth, worldHeight;
    [SerializeField]
    GameObject dirtTilePrefab, grassTilePrefab,concreteTilePrefab;
    [SerializeField]
    private IsoGrid myIsoGrid;

    int tileLayerStartingPoint = -10000;

    System.Random rnd;

    public uint GridWidth { get => gridWidth; }
    public uint GridHeight { get => gridHeight; }
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        rnd = new System.Random((int)(Time.time*Time.time)*Time.frameCount);
    }
    /// <summary>
    /// generates map tiles with random sprites based on width and height
    /// </summary>
    public void GenerateMap()
    {
        if (myIsoGrid == null)
        {
            return;
        }
        myIsoGrid.InitIsoGrid(gridWidth, gridHeight);
        List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
        for (uint i = 0; i < gridHeight; i++)
        {
            for (uint j = 0; j < gridWidth; j++)
            {
                tileType current = myIsoGrid.GetMapPoint(j, i);
                GameObject newTile = null;
                switch (current)
                {
                    case tileType.grass:
                        newTile = GameObject.Instantiate(grassTilePrefab, myIsoGrid.ToWorldSpace(j, i), this.transform.rotation, this.transform);
                        break;
                    case tileType.dirt:
                        newTile = GameObject.Instantiate(dirtTilePrefab, myIsoGrid.ToWorldSpace(j, i), this.transform.rotation, this.transform);
                        break;
                }
                newTile.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)(tileLayerStartingPoint - i + j);

            }
        }
    }
    /// <summary>
    /// generates map tiles with random sprites based on width and height
    /// </summary>
    public void GenerateMap(uint width, uint height)
    {
        if (myIsoGrid == null)
        {
            return;
        }
        gridWidth = width;
        gridHeight = height;
        myIsoGrid.InitIsoGrid(gridWidth, gridHeight);

        RandomizeTiles(myIsoGrid);

        for (uint i = 0; i < gridHeight; i++)
        {
            for (uint j = 0; j < gridWidth; j++)
            {
                tileType current = myIsoGrid.GetMapPoint(j, i);
                GameObject newTile = null;
                switch (current)
                {
                    case tileType.grass:
                        newTile = GameObject.Instantiate(grassTilePrefab, myIsoGrid.ToWorldSpace(j, i), this.transform.rotation, this.transform);

                        break;
                    case tileType.dirt:
                        newTile = GameObject.Instantiate(dirtTilePrefab, myIsoGrid.ToWorldSpace(j, i), this.transform.rotation, this.transform);

                        break;
                    case tileType.concrete:
                        newTile = GameObject.Instantiate(concreteTilePrefab, myIsoGrid.ToWorldSpace(j, i), this.transform.rotation, this.transform);

                        break;
                }
                newTile.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)(tileLayerStartingPoint - i + j);

            }
        }
    }
    /// <summary>
    /// Create roads at random positions then fill the rest with random nature tiles
    /// </summary>
    /// <param name="generateOver"></param>
    private void RandomizeTiles(IsoGrid generateOver)
    {
        rnd = new System.Random((int)(Time.time * Time.time) * Time.frameCount);

        bool twoRoads = true;
        if (rnd.NextDouble() >= 0.5f)
        {
            twoRoads = false;
        }
        int roadX = -10, roadY = -10;
        if (twoRoads)
        {
            roadX = rnd.Next((int)generateOver.GridWidth);
            roadY = rnd.Next((int)generateOver.GridHeight);


        }
        else
        {
            //create only one road at random side
            if(rnd.NextDouble() >= 0.5d)
            {
                roadX = rnd.Next((int)generateOver.GridWidth);

            }
            else
            {
                roadY = rnd.Next((int)generateOver.GridHeight);

            }
        }
        for (uint y = 0; y < generateOver.GridHeight; y++)
        {
            for (uint x = 0; x < generateOver.GridWidth; x++)
            {
                if (x <= roadX + 1 && x >= roadX - 1)
                {
                    generateOver.SetMapPoint(x, y, tileType.concrete);
                }
                else
                {
                    if(y <= roadY + 1 && y >= roadY - 1)
                    {
                        generateOver.SetMapPoint(x, y, tileType.concrete);

                    }
                    else
                    {
                        //not a road, generate random nature tile
                        if (rnd.NextDouble() >= 0.5d)
                        {
                            generateOver.SetMapPoint(x, y, tileType.grass);
                        }
                        else
                        {
                            generateOver.SetMapPoint(x, y, tileType.dirt);

                        }
                    }
                }
            }
        }
    }

}
