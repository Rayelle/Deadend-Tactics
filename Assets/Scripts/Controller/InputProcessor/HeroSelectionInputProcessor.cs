using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectionInputProcessor : InputProcessor
{
    public static HeroSelectionInputProcessor instance;

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
    public override void Init()
    {
        InputManager.instance.CurrentInputProcessor = HeroSelectionInputProcessor.instance;
        InputManager.instance.CurrentDirectionInputProcessor = SpaceSelectorDirectionProcessor.instance;
    }
    /// <summary>
    /// Select hero at cursor position
    /// </summary>
    public override void Accept()
    {
        //if space contains a hero, select it and switch to movement-phase
        if (MapContent.instance.SpaceContainsHero(SpaceSelectorDirectionProcessor.instance.HighlightPos))
        {
            if (MapContent.instance.Dictionary[SpaceSelectorDirectionProcessor.instance.HighlightPos].Ready)
            {
                HeroManager.instance.SelectedHero = MapContent.instance.Dictionary[SpaceSelectorDirectionProcessor.instance.HighlightPos];
                InputManager.instance.CurrentInputProcessor = MovementInputProcessor.instance;
                MovementInputProcessor.instance.StartMovementPhase();
                AudioManager.instance.PlayMenuAcceptSound();
            }

        }
    }
    public override void Refuse()
    {
       
    }
}
