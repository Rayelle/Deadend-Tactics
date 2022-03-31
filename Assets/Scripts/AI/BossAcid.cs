using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for managing tiles affected by boss-acid
/// tiles affected by boss-acid will deal damage to heroes standing on them at the start of the ai-turn
/// </summary>
public class BossAcid : MonoBehaviour
{
    public static BossAcid instance;

    [SerializeField]
    GameObject acidTilePrefab;
    [SerializeField]
    float acidDamage;
    HashSet<Vector2Int> allAcidSpaces = new HashSet<Vector2Int>();
    List<GameObject> allTiles = new List<GameObject>();

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
    private void Start()
    {
        GameEvents.instance.onEndPlayerTurn += ResolveAcidDamage;
    }
    private void OnDisable()
    {
        GameEvents.instance.onEndPlayerTurn -= ResolveAcidDamage;
        ResetAcidSpaces();
    }

    /// <summary>
    /// resolves damage for all acid spaces
    /// </summary>
    private void ResolveAcidDamage()
    {
        foreach (Vector2Int acidSpace in allAcidSpaces)
        {
            if (MapContent.instance.SpaceContainsHero(acidSpace))
            {
                MapContent.instance.Dictionary[acidSpace].TakeDamage(acidDamage);
            }
        }
    }
    /// <summary>
    /// Create a new acid-tile and add it to the collection
    /// </summary>
    /// <param name="acidSpace"></param>
    public void AddAcidSpace(Vector2Int acidSpace)
    {
        if (IsoGrid.instance.IsInsideBounds(acidSpace))
        {
            if (!MapContent.instance.SpaceContainsObstacle(acidSpace))
            {
                allTiles.Add(GameObject.Instantiate(acidTilePrefab, IsoGrid.instance.ToWorldSpace(acidSpace), this.transform.rotation, this.transform));
                allAcidSpaces.Add(acidSpace);
            }
        }
    }
    /// <summary>
    /// Disable all acid-tiles
    /// </summary>
    public void ResetAcidSpaces()
    {
        foreach (GameObject tile in allTiles)
        {
            Destroy(tile);
        }
        allTiles = new List<GameObject>();
        allAcidSpaces = new HashSet<Vector2Int>();
    }
    
}
