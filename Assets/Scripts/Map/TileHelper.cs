using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHelper: MonoBehaviour
{
    public static TileHelper instance;

    private int tileOverlayStartingPoint =- 100;

    public int TileOverlayStartingPoint { get => tileOverlayStartingPoint; }

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
    /// <summary>
    /// Spawn ranged highlights in current map around given position. 
    /// </summary>
    /// <returns>list of spawned gameObjects</returns>
    public List<GameObject> SpawnHighlightsAround(Vector2Int position,int range, GameObject highlightPrefab,Transform parent)
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        List<Vector2Int> spacesInRange = ZombieHelper.GetSpacesInRange(position, range);

        foreach (Vector2Int spaceInRange in spacesInRange)
        {
            GameObject newHighlight = Instantiate(highlightPrefab, IsoGrid.instance.ToWorldSpace((uint)spaceInRange.x, (uint)spaceInRange.y), this.transform.rotation ,parent);

            spawnedObjects.Add(newHighlight);
            newHighlight.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)(tileOverlayStartingPoint - spaceInRange.y + spaceInRange.x);
        }

        return spawnedObjects;
    }
    /// <summary>
    /// Spawn ranged highlights in current map around given position. 
    /// </summary>
    /// <param name="spacesInRange"></param>
    /// <param name="highlightPrefab"></param>
    /// <param name="parent"></param>
    /// <returns>list of spawned gameObjects</returns>
    public List<GameObject> SpawnHighlightsAround(List<Vector2Int> spacesInRange, GameObject highlightPrefab, Transform parent)
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        foreach (Vector2Int spaceInRange in spacesInRange)
        {
            GameObject newHighlight = Instantiate(highlightPrefab, IsoGrid.instance.ToWorldSpace((uint)spaceInRange.x, (uint)spaceInRange.y), this.transform.rotation, parent);

            spawnedObjects.Add(newHighlight);
            newHighlight.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)(tileOverlayStartingPoint - spaceInRange.y + spaceInRange.x);
        }

        return spawnedObjects;
    }
}
