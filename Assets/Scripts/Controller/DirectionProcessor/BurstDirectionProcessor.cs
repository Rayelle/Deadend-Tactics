using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstDirectionProcessor : DirectionProcessor
{
    public static BurstDirectionProcessor instance;

    [SerializeField]
    GameObject rangeIndicatorPrefab;
    List<GameObject> allRangeHighlights=new List<GameObject>();
    Vector2Int currentSelectedSpace;
    List<Vector2Int> spacesInBurstRange = new List<Vector2Int>();

    public List<Vector2Int> SpacesInBurstRange { get => spacesInBurstRange; }

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
    /// Select burst-direction based on wether surrounding spaces are part of current map.
    /// Priority is upwards, downwards, rightwards, leftwards.
    /// </summary>
    /// <param name="highlightStartingPosition"></param>
    public override void StartHighlight(Vector2Int highlightStartingPosition)
    {

        if(IsoGrid.instance.IsInsideBounds(HeroManager.instance.SelectedHero.gridPosition + new Vector2Int(1, 0)))
        {
            MoveHighlightUp();
        }
        else
        {
            if(IsoGrid.instance.IsInsideBounds(HeroManager.instance.SelectedHero.gridPosition + new Vector2Int(-1, 0)))
            {
                MoveHighlightDown();
            }
            else
            {
                if(IsoGrid.instance.IsInsideBounds(HeroManager.instance.SelectedHero.gridPosition + new Vector2Int(0, 1)))
                {
                    MoveHighlightRight();
                }
                else
                {
                    MoveHighlightLeft();
                }
            }
        }
    }
    /// <summary>
    /// Destroy all current attack-highlight.
    /// </summary>
    public override void EndHighlight()
    {
        foreach (GameObject highlight in allRangeHighlights)
        {
            Destroy(highlight);
        }
        allRangeHighlights = new List<GameObject>();
        spacesInBurstRange = new List<Vector2Int>();
    }
    /// <summary>
    /// Change all spaces inside burst range and display attack-highlight.
    /// </summary>
    public override void MoveHighlightUp()
    {

        Vector2Int direction = HeroManager.instance.SelectedHero.gridPosition + new Vector2Int(0, 1);
        if (IsoGrid.instance.IsInsideBounds(direction))
        {
            EndHighlight();
            currentSelectedSpace = direction;
            spacesInBurstRange.Add(currentSelectedSpace);
            Vector2Int positonToHighlight = currentSelectedSpace + new Vector2Int(0, 1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(1, 0);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(-1, 0);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(-1, 1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(1, 1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
        }
        foreach (Vector2Int space in spacesInBurstRange)
        {
            allRangeHighlights.Add(GameObject.Instantiate(rangeIndicatorPrefab, IsoGrid.instance.ToWorldSpace(space), this.transform.rotation, this.transform));
        }
    }
    /// <summary>
    /// Change all spaces inside burst range and display attack-highlight.
    /// </summary>
    public override void MoveHighlightDown()
    {
        Vector2Int direction = HeroManager.instance.SelectedHero.gridPosition + new Vector2Int(0, -1);
        if (IsoGrid.instance.IsInsideBounds(direction))
        {
            EndHighlight();
            currentSelectedSpace = direction;
            spacesInBurstRange.Add(currentSelectedSpace);
            Vector2Int positonToHighlight = currentSelectedSpace + new Vector2Int(0, -1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(1, 0);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(-1, 0);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(1, -1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(-1, -1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
        }
        foreach (Vector2Int space in spacesInBurstRange)
        {
            allRangeHighlights.Add(GameObject.Instantiate(rangeIndicatorPrefab, IsoGrid.instance.ToWorldSpace(space), this.transform.rotation, this.transform));
        }
    }
    /// <summary>
    /// Change all spaces inside burst range and display attack-highlight.
    /// </summary>
    public override void MoveHighlightLeft()
    {
        Vector2Int direction = HeroManager.instance.SelectedHero.gridPosition + new Vector2Int(-1, 0);
        if (IsoGrid.instance.IsInsideBounds(direction))
        {
            EndHighlight();
            currentSelectedSpace = direction;
            spacesInBurstRange.Add(currentSelectedSpace);
            Vector2Int positonToHighlight = currentSelectedSpace + new Vector2Int(-1, 0);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(-1, 1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(-1, -1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(0, 1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(0, -1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
        }
        foreach (Vector2Int space in spacesInBurstRange)
        {
            allRangeHighlights.Add(GameObject.Instantiate(rangeIndicatorPrefab, IsoGrid.instance.ToWorldSpace(space), this.transform.rotation, this.transform));
        }
       
    }
    /// <summary>
    /// Change all spaces inside burst range and display attack-highlight.
    /// </summary>
    public override void MoveHighlightRight()
    {
        Vector2Int direction = HeroManager.instance.SelectedHero.gridPosition + new Vector2Int(1, 0);
        if (IsoGrid.instance.IsInsideBounds(direction))
        {
            EndHighlight();
            currentSelectedSpace = direction;
            spacesInBurstRange.Add(currentSelectedSpace);
            Vector2Int positonToHighlight = currentSelectedSpace + new Vector2Int(1, 0);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(1, 1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(1, -1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(0, 1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
            positonToHighlight = currentSelectedSpace + new Vector2Int(0, -1);
            if (IsoGrid.instance.IsInsideBounds(positonToHighlight))
                spacesInBurstRange.Add(positonToHighlight);
        }
        foreach (Vector2Int space in spacesInBurstRange)
        {
            allRangeHighlights.Add(GameObject.Instantiate(rangeIndicatorPrefab, IsoGrid.instance.ToWorldSpace(space), this.transform.rotation, this.transform));
        }
    }
}
