using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HeroEnums;

public class ActionMenuDirectionProcessor : DirectionProcessor
{
    [SerializeField]
    GameObject menuParent;
    //[SerializeField]
    //SpriteRenderer[] allArrows;
    [SerializeField]
    Sprite[] menuSpriteOptions;
    [SerializeField]
    SpriteRenderer myMenuSpriteRenderer;

    protected int currentSelection=0;

    private bool active = false;

    public int CurrentSelection { get => currentSelection; }


    /// <summary>
    /// Starts the MenuSelectionHighlight
    /// fill parameter with dummy
    /// </summary>
    /// <param name="HighlightStatingPosition"></param>
    public override void StartHighlight(Vector2Int HighlightStatingPosition)
    {
        currentSelection = 0;
        myMenuSpriteRenderer.sprite = menuSpriteOptions[0];
        active = true;

        menuParent.gameObject.SetActive(true);
        

        //allArrows[currentSelection].gameObject.SetActive(true);
        
    }
    /// <summary>
    /// Deactivate Menu and selection-arrows.
    /// </summary>
    public override void EndHighlight()
    {
        active = false;
        menuParent.SetActive(false);

    }
    /// <summary>
    /// Change current selection and selection-arrow.
    /// </summary>
    public override void MoveHighlightUp()
    {
        if (active)
        {
            if (currentSelection - 1 >= 0)
            {
                currentSelection--;
                myMenuSpriteRenderer.sprite = menuSpriteOptions[currentSelection];
                AudioManager.instance.PlayMenuMoveSound();
            }
        }
    }
    /// <summary>
    /// Change current selection and selection-arrow.
    /// </summary>
    public override void MoveHighlightDown()
    {
        if (active)
        {
            if (currentSelection + 1 < menuSpriteOptions.Length)
            {
                currentSelection++;
                myMenuSpriteRenderer.sprite = menuSpriteOptions[currentSelection];
                AudioManager.instance.PlayMenuMoveSound();

            }
        }
    }
    /// <summary>
    /// Is always overriden in child-class based on hero-type.
    /// </summary>
    /// <returns></returns>
    public virtual HeroActions returnAction()
    {
        return HeroActions.wait;
    }

}
