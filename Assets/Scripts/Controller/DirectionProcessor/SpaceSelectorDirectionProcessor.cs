using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSelectorDirectionProcessor : DirectionProcessor
{
    public static SpaceSelectorDirectionProcessor instance;
    [SerializeField]
    SpriteRenderer mySpriteRenderer;

    private Vector2Int highlightPos;
    private bool active = false;

    public Vector2Int HighlightPos { get => highlightPos; }

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


    private void Start()
    {
        GameEvents.instance.onEndPlayerTurn += EndPlayerTurn;
        GameEvents.instance.onEndAITurn += EndAITurn;
    }
    private void OnDisable()
    {
        GameEvents.instance.onEndPlayerTurn -= EndPlayerTurn;
        GameEvents.instance.onEndAITurn -= EndAITurn;
    }
    /// <summary>
    /// Called by end-player-turn-event.
    /// </summary>
    private void EndPlayerTurn()
    {
        EndHighlight();
    }

    private void EndAITurn()
    {
        mySpriteRenderer.enabled = true;
        active = true;
    }

    public override void StartHighlight(Vector2Int highlightStartingPosition)
    {
        //set up spaceHighlighter at a given position
        if (!IsoGrid.instance.IsInsideBounds(highlightStartingPosition))
        {
            return;
        }
        highlightPos = highlightStartingPosition;
        mySpriteRenderer.enabled = true;
        active = true;
        this.transform.position = IsoGrid.instance.ToWorldSpace((uint)highlightPos.x, (uint)highlightPos.y);
        mySpriteRenderer.sortingOrder = TileHelper.instance.TileOverlayStartingPoint + 1 - highlightPos.y + highlightPos.x;
    }
    /// <summary>
    /// Deactivates space-selection-highlight.
    /// </summary>
    public override void EndHighlight()
    {
        mySpriteRenderer.enabled = false;
        active = false;
    }
    /// <summary>
    /// moves currently selected space upwards.
    /// </summary>
    public override void MoveHighlightUp()
    {
        if (active)
        {
            if(IsoGrid.instance.IsInsideBounds(new Vector2Int(highlightPos.x, highlightPos.y+1)))
            {
                highlightPos.y += 1;
                this.transform.position = IsoGrid.instance.ToWorldSpace((uint)highlightPos.x, (uint)highlightPos.y);
                DisplayTileInformation();
            }
        }
    }
    /// <summary>
    /// moves currently selected space downwards.
    /// </summary>
    public override void MoveHighlightDown()
    {
        if (active)
        {
            if (IsoGrid.instance.IsInsideBounds(new Vector2Int(highlightPos.x, highlightPos.y - 1)))
            {
                highlightPos.y -= 1;
                this.transform.position = IsoGrid.instance.ToWorldSpace((uint)highlightPos.x, (uint)highlightPos.y);
                DisplayTileInformation();
            }

        }
    }
    /// <summary>
    /// moves currently selected space leftwards.
    /// </summary>
    public override void MoveHighlightLeft()
    {
        if (active)
        {
            if (IsoGrid.instance.IsInsideBounds(new Vector2Int(highlightPos.x-1, highlightPos.y )))
            {
                highlightPos.x -= 1;
                this.transform.position = IsoGrid.instance.ToWorldSpace((uint)highlightPos.x, (uint)highlightPos.y);
                DisplayTileInformation();
            }

        }
    }
    /// <summary>
    /// moves currently selected space rightwards.
    /// </summary>
    public override void MoveHighlightRight()
    {
        if (active)
        {
            if (IsoGrid.instance.IsInsideBounds(new Vector2Int(highlightPos.x+1, highlightPos.y)))
            {
                highlightPos.x += 1;
                this.transform.position = IsoGrid.instance.ToWorldSpace((uint)highlightPos.x, (uint)highlightPos.y);
                DisplayTileInformation();
            }
        }
    }

    /// <summary>
    /// Display information of currently selected tile
    /// </summary>
    private void DisplayTileInformation()
    {
        if (MapContent.instance.Dictionary.ContainsKey(highlightPos))
        {
            MapContent.instance.Dictionary[highlightPos].MyTileInformation.ApplyTileInformation();
        }
        else
        {
            TileInformationSetup.instance.HideGUIDescription();
        }
    }
}
