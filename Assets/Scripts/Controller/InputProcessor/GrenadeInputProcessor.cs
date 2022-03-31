using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeInputProcessor : InputProcessor
{
    public static GrenadeInputProcessor instance;

    //public Unit CurrentSelection { set => currentSelection = value; }

    [SerializeField]
    GameObject rangeIndicatorPrefab,grenadePrefab;
    //[SerializeField]
    //Hero myGunner;
    [SerializeField]
    float throwAnimationDuration, grenadeDamageDelay;
    int tileOverlayStartingPoint = -100;

    private List<GameObject> allRangeHighlights = new List<GameObject>();
    private HashSet<Vector2Int> positionsInRange;

    private void Awake()
    {
        //singleton
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
    /// Set up grenade throw
    /// </summary>
    public override void Init()
    {
        InputManager.instance.CurrentInputProcessor = GrenadeInputProcessor.instance;
        InputManager.instance.CurrentDirectionInputProcessor = SpaceSelectorDirectionProcessor.instance;

        positionsInRange = MapContent.instance.getSpacesInRange(HeroManager.instance.SelectedHero.gridPosition, HeroStatistics.GunnerGrenadeRange);
        foreach (Vector2Int spaceInGrenadeRange in positionsInRange)
        {
            GameObject newHighlight = Instantiate(rangeIndicatorPrefab, IsoGrid.instance.ToWorldSpace(spaceInGrenadeRange), this.transform.rotation, this.transform);
            newHighlight.GetComponentInChildren<SpriteRenderer>().sortingOrder= (int)(tileOverlayStartingPoint - spaceInGrenadeRange.y + spaceInGrenadeRange.x);
            allRangeHighlights.Add(newHighlight);
        }

    }


    /// <summary>
    /// Throw grenade at selected space. All enemies in range of grenade blast will take damage.
    /// </summary>
    public override void Accept()
    {
        StartCoroutine(ThrowAnimation(HeroManager.instance.SelectedHero));
    }
    /// <summary>
    /// end activation
    /// </summary>
    public override void Refuse()
    {
        InputManager.instance.CurrentInputProcessor = HeroSelectionInputProcessor.instance;
        GameEvents.instance.EndActivation(HeroManager.instance.SelectedHero);
        EndRangeHighlight();
    }
    /// <summary>
    /// Animate grenade throw towards chosen tile and create an explosion animation at the right moment
    /// </summary>
    /// <param name="myGunner"></param>
    /// <returns></returns>
    private IEnumerator ThrowAnimation(Unit myGunner)
    {
        //deactivate control and play animation
        InputManager.instance.DeactivatePlayerControl();
        if (myGunner.MyAnimator != null)
        {
            myGunner.MyAnimator.SetBool("grenade", true);
        }
        yield return new WaitForSeconds(throwAnimationDuration);

        if (myGunner.MyAnimator != null)
        {
            myGunner.MyAnimator.SetBool("grenade", false);
        }
        //after animation has finished, create explosionPrefab at currently selected tile
        GameObject.Instantiate(grenadePrefab, IsoGrid.instance.ToWorldSpace(SpaceSelectorDirectionProcessor.instance.HighlightPos), Quaternion.identity);
        yield return new WaitForSeconds(grenadeDamageDelay);
        //after delay deal damage to all enemies inside explosion range
        if (positionsInRange.Contains(SpaceSelectorDirectionProcessor.instance.HighlightPos))
        {
            foreach (Vector2Int spaceInExplosion in MapContent.instance.getSpacesInRange(SpaceSelectorDirectionProcessor.instance.HighlightPos, HeroStatistics.GunnerGrenadeExplosionRange))
            {
                Unit currentEnemy;
                if (MapContent.instance.tryGetEnemy(spaceInExplosion, out currentEnemy))
                {
                    if (HeroStatistics.GunnerStunGrenade)
                    {
                        currentEnemy.Stunned = true;
                    }
                    else
                    {
                        currentEnemy.TakeDamage(HeroStatistics.GunnerGrenadeDamage);
                    }

                }
            }
            InputManager.instance.CurrentInputProcessor = HeroSelectionInputProcessor.instance;
            EndRangeHighlight();
            GameEvents.instance.EndActivation(HeroManager.instance.SelectedHero);
            InputManager.instance.ActivatePlayerControl();

        }
    }
    /// <summary>
    /// Destroy all gameObjects of range-highlight
    /// </summary>
    private void EndRangeHighlight()
    {
        foreach (GameObject current in allRangeHighlights)
        {
            Destroy(current);
        }
        allRangeHighlights = new List<GameObject>();
    }
    /// <summary>
    /// returns a list of all spaces inside grenade explosion range. 
    /// </summary>
    /// <returns></returns>
    private List<Vector2Int> SpacesInExplosionRange()
    {
        List<Vector2Int> spacesInExplosionRange = new List<Vector2Int>();
        spacesInExplosionRange.Add(SpaceSelectorDirectionProcessor.instance.HighlightPos);
        for (int i = 0; i < HeroStatistics.GunnerGrenadeExplosionRange; i++)
        {
            int numberOfLoops = spacesInExplosionRange.Count;
            for (int j = 0; j < numberOfLoops; j++)
            {
                Vector2Int newPosition = new Vector2Int(spacesInExplosionRange[j].x + 1, spacesInExplosionRange[j].y);
                if (!spacesInExplosionRange.Contains(newPosition))
                {
                    if (IsoGrid.instance.IsInsideBounds(newPosition))
                        spacesInExplosionRange.Add(newPosition);
                }
                newPosition = new Vector2Int(spacesInExplosionRange[j].x - 1, spacesInExplosionRange[j].y);
                if (!spacesInExplosionRange.Contains(newPosition))
                {
                    if (IsoGrid.instance.IsInsideBounds(newPosition))
                        spacesInExplosionRange.Add(newPosition);
                }
                newPosition = new Vector2Int(spacesInExplosionRange[j].x, spacesInExplosionRange[j].y + 1);
                if (!spacesInExplosionRange.Contains(newPosition))
                {
                    if (IsoGrid.instance.IsInsideBounds(newPosition))
                        spacesInExplosionRange.Add(newPosition);
                }
                newPosition = new Vector2Int(spacesInExplosionRange[j].x, spacesInExplosionRange[j].y - 1);
                if (!spacesInExplosionRange.Contains(newPosition))
                {
                    if (IsoGrid.instance.IsInsideBounds(newPosition))
                        spacesInExplosionRange.Add(newPosition);
                }
            }
        }

        return spacesInExplosionRange;
    }
}
