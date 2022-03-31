 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEnums;

public class MovementInputProcessor : InputProcessor
{
    public static MovementInputProcessor instance;

    [SerializeField]
    GameObject movementHighlightPrefab;
    int tileOverlayStartingPoint=-100;
    //[SerializeField]
    //AttackInputProcessor myFollowup;

    ActionMenuDirectionProcessor gunnerMenuSelectionHighlight, medicMenuSelectionHighlight, tankMenuSelectionHighlight;

    List<GameObject> currentHighlights = new List<GameObject>();

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
        GameEvents.instance.onSpawnUnit += RegisterHeroMenu;
    }
    private void OnDisable()
    {
        GameEvents.instance.onSpawnUnit -= RegisterHeroMenu;

    }
    /// <summary>
    /// Registers action-menu for each hero.
    /// 
    /// </summary>
    /// <param name="newUnit"></param>
    private void RegisterHeroMenu(Unit newUnit)
    {
        switch (newUnit.MyUnitType)
        {
            case UnitType.gunner:
                gunnerMenuSelectionHighlight = newUnit.GetComponent<ActionMenuDirectionProcessor>();
                break;
            case UnitType.tank:
                tankMenuSelectionHighlight = newUnit.GetComponent<ActionMenuDirectionProcessor>();
                break;
            case UnitType.medic:
                medicMenuSelectionHighlight = newUnit.GetComponent<ActionMenuDirectionProcessor>();
                break;
        }
        
    }
    /// <summary>
    /// Move selected hero to space if possible
    /// </summary>
    public override void Accept()
    {
        //if space is reachable and inside the movement range of the hero, move hero towards space
        PathfindingNode destinationNode = Pathfinding.FindPath(HeroManager.instance.SelectedHero.gridPosition, SpaceSelectorDirectionProcessor.instance.HighlightPos);
        if (destinationNode != null)
        {
            if (destinationNode.stepsToPathtaker <= HeroManager.instance.SelectedHero.MoveRange)
            {

                AudioManager.instance.PlayMenuAcceptSound();

                StartCoroutine(MoveStepByStep(destinationNode));
                return;
            }
        }

        //could not go to space, play error sound
        AudioManager.instance.PlayMenuErrorSound();


        
    }
    /// <summary>
    /// animate walk and move hero towards position
    /// </summary>
    /// <param name="destinationNode"></param>
    /// <returns></returns>
    private IEnumerator MoveStepByStep(PathfindingNode destinationNode)
    {
        InputManager.instance.DeactivatePlayerControl();
        if (HeroManager.instance.SelectedHero.MyAnimator != null)
        {
            HeroManager.instance.SelectedHero.MyAnimator.SetBool("walking", true);
        }
        foreach (PathfindingNode current in destinationNode.EachStepToNode())
        {
            HeroManager.instance.SelectedHero.MoveUnitTo(current.Position);

            yield return new WaitForSeconds(HeroManager.instance.HeroMoveDelay);
        }
        if (HeroManager.instance.SelectedHero.MyAnimator != null)
        {
            HeroManager.instance.SelectedHero.MyAnimator.SetBool("walking", false);
        }
        InputManager.instance.ActivatePlayerControl();
        ChangeToInputMenu();
    }
    /// <summary>
    /// when a hero is moved, this function is called.
    /// Display action-menu of current hero and change current Input-Processors in InputManager.
    /// </summary>
    private void ChangeToInputMenu()
    {
        EndMovementPhase();
        InputManager.instance.CurrentInputProcessor = ActionMenuInputProcessor.instance;
        switch (HeroManager.instance.SelectedHero.MyUnitType)
        {
            case UnitType.gunner:
                InputManager.instance.CurrentDirectionInputProcessor = gunnerMenuSelectionHighlight;
                gunnerMenuSelectionHighlight.StartHighlight(Vector2Int.one);
                ActionMenuInputProcessor.instance.MyMenuSelection = gunnerMenuSelectionHighlight;
                break;
            case UnitType.tank:
                InputManager.instance.CurrentDirectionInputProcessor = tankMenuSelectionHighlight;
                tankMenuSelectionHighlight.StartHighlight(Vector2Int.one);
                ActionMenuInputProcessor.instance.MyMenuSelection = tankMenuSelectionHighlight;
                break;
            case UnitType.medic:
                InputManager.instance.CurrentDirectionInputProcessor = medicMenuSelectionHighlight;
                medicMenuSelectionHighlight.StartHighlight(Vector2Int.one);
                ActionMenuInputProcessor.instance.MyMenuSelection = medicMenuSelectionHighlight;
                break;
        }
    }
    /// <summary>
    /// Deselect hero when back-button is pressed.
    /// </summary>
    public override void Refuse()
    {
        HeroManager.instance.SelectedHero = null;
        InputManager.instance.CurrentInputProcessor = HeroSelectionInputProcessor.instance;
        EndMovementPhase();
    }
    /// <summary>
    /// Start movement-phase by displaying possible moves
    /// </summary>
    public void StartMovementPhase()
    {
        List<Vector2Int> spacesInRange = ZombieHelper.GetSpacesInRange(HeroManager.instance.SelectedHero.gridPosition, HeroManager.instance.SelectedHero.MoveRange);
        foreach (Vector2Int spaceInRange in spacesInRange)
        {
            PathfindingNode possiblePath = Pathfinding.FindPath(HeroManager.instance.SelectedHero.gridPosition, spaceInRange);
            if (possiblePath != null)
            {
                if (possiblePath.stepsToPathtaker <= HeroManager.instance.SelectedHero.MoveRange)
                {
                    //spacesInMoveRange.Add(spaceInRange);
                    GameObject newHighlight = GameObject.Instantiate(movementHighlightPrefab, IsoGrid.instance.ToWorldSpace(spaceInRange), this.transform.rotation, this.transform);
                    currentHighlights.Add(newHighlight);
                    newHighlight.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)(tileOverlayStartingPoint - spaceInRange.y + spaceInRange.x);
                }

            }
        }

    }
    /// <summary>
    /// destroy all highlight-tiles
    /// </summary>
    public void EndMovementPhase()
    {
        foreach (GameObject highlightTile in currentHighlights)
        {
            Destroy(highlightTile);
        }
    }
}
