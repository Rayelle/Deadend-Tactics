using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEnums;

public class ActionMenuInputProcessor : InputProcessor
{
    public static ActionMenuInputProcessor instance;

    ActionMenuDirectionProcessor myMenuSelection;

    public ActionMenuDirectionProcessor MyMenuSelection { set => myMenuSelection = value; }

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
    /// Set up a selected action in InputManager.
    /// </summary>
    public override void Accept()
    {
        AudioManager.instance.PlayMenuAcceptSound();

        //end menu after choice is selected
        InputManager.instance.CurrentDirectionInputProcessor.EndHighlight();

        //look for which action has been selected each hero returns different actions
        HeroActions currentAction = myMenuSelection.returnAction();
        switch (currentAction)
        {
            case HeroActions.wait:
                EndHeroActivation();
                break;

            case HeroActions.shoot:
                //start shoot-phase
                AttackInputProcessor.instance.Init();
                break;

            case HeroActions.frag:
                if (HeroManager.instance.GunnerGrenadeCooldown == 0)
                {
                    //star grenade-phase
                    HeroManager.instance.GunnerGrenadeCooldown = HeroManager.instance.GunnerGrenadeMaxCooldown;
                    GrenadeInputProcessor.instance.Init();
                }
                else
                {
                    //stay in menu
                    InputManager.instance.CurrentDirectionInputProcessor.StartHighlight(Vector2Int.up);
                }
                break;

            case HeroActions.block:
                if (HeroManager.instance.TankBlockCooldown == 0)
                {
                    //start block animation
                    HeroManager.instance.TankBlockCooldown = HeroManager.instance.TankBlockMaxCooldown;
                    TankBlock.instance.ActivateTankBlock();
                    EndHeroActivation();
                }
                else
                {
                    //stay in menu
                    InputManager.instance.CurrentDirectionInputProcessor.StartHighlight(Vector2Int.up);
                }
                break;
            case HeroActions.heal:
                if (HeroManager.instance.MedicHealCooldown == 0)
                {
                    //start heal-animation
                    HeroManager.instance.MedicHealCooldown = HeroManager.instance.MedicHealMaxCooldown;
                    MedicHeal.instance.ActivateMedicHeal();
                    EndHeroActivation();

                }
                else
                {
                    //stay in menu
                    InputManager.instance.CurrentDirectionInputProcessor.StartHighlight(Vector2Int.up);
                }
                break;
            case HeroActions.burst:
                if (HeroManager.instance.TankBurstCooldown == 0)
                {
                    HeroManager.instance.TankBurstCooldown = HeroManager.instance.TankBurstMaxCooldown;
                    BurstInputProcessor.instance.Init();

                }
                else
                {
                    //stay in menu
                    InputManager.instance.CurrentDirectionInputProcessor.StartHighlight(Vector2Int.up);
                }
                break;
        }
    }
    /// <summary>
    /// Ends hero activation.
    /// </summary>
    public override void Refuse()
    {
        //wait-action
        InputManager.instance.CurrentDirectionInputProcessor.EndHighlight();
        EndHeroActivation();

    }
    /// <summary>
    /// end the current heroes activation
    /// </summary>
    private void EndHeroActivation()
    {
        HeroSelectionInputProcessor.instance.Init();
        GameEvents.instance.EndActivation(HeroManager.instance.SelectedHero);
    }
}
