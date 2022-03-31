using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstInputProcessor : InputProcessor
{
    public static BurstInputProcessor instance;
    float delayFirstSecondShot=0.3f, delaySecondShotEnd=0.5f;

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
        InputManager.instance.CurrentInputProcessor = this;
        InputManager.instance.CurrentDirectionInputProcessor = BurstDirectionProcessor.instance;
    }

    /// <summary>
    /// Attack into selected direction, all enemies inside burst-area will be damaged.
    /// </summary>
    public override void Accept()
    {
        StartCoroutine(ActivateBurst());

    }
    /// <summary>
    /// Animates burst in selected direction, deals damage to all enemies inside its range
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActivateBurst()
    {
        //deactivate control and play animation
        InputManager.instance.DeactivatePlayerControl();
        if (HeroManager.instance.SelectedHero.MyAnimator != null)
        {
            HeroManager.instance.SelectedHero.MyAnimator.SetBool("burst", true);
        }
        yield return new WaitForSeconds(HeroManager.instance.SelectedHero.AttackSoundEffectDelay);
        //sound-effect is played twice
        if (HeroManager.instance.SelectedHero.AttackAudio != null)
        {
            HeroManager.instance.SelectedHero.AttackAudio.Play();
        }
        yield return new WaitForSeconds(delayFirstSecondShot);
        if (HeroManager.instance.SelectedHero.AttackAudio != null)
        {
            HeroManager.instance.SelectedHero.AttackAudio.Play();
        }
        //deal damage to enemies inside current burst range
        foreach (Vector2Int space in BurstDirectionProcessor.instance.SpacesInBurstRange)
        {
            if (MapContent.instance.Dictionary.ContainsKey(space))
            {
                Unit unitInSpace = MapContent.instance.Dictionary[space];
                if (unitInSpace.MyUnitType == HeroEnums.UnitType.enemy)
                {
                    unitInSpace.TakeDamage(HeroStatistics.TankBurstDamage);
                }
            }
        }

        yield return new WaitForSeconds(delaySecondShotEnd);
        //stop animation and end the heroes activation
        if (HeroManager.instance.SelectedHero.MyAnimator != null)
        {
            HeroManager.instance.SelectedHero.MyAnimator.SetBool("burst", false);
        }

        InputManager.instance.CurrentDirectionInputProcessor = SpaceSelectorDirectionProcessor.instance;
        InputManager.instance.CurrentInputProcessor = HeroSelectionInputProcessor.instance;
        GameEvents.instance.EndActivation(HeroManager.instance.SelectedHero);
        BurstDirectionProcessor.instance.EndHighlight();

        InputManager.instance.ActivatePlayerControl();
    }
    /// <summary>
    /// hero ends activation.
    /// </summary>
    public override void Refuse()
    {
        InputManager.instance.CurrentDirectionInputProcessor = SpaceSelectorDirectionProcessor.instance;
        InputManager.instance.CurrentInputProcessor = HeroSelectionInputProcessor.instance;
        GameEvents.instance.EndActivation(HeroManager.instance.SelectedHero);
        BurstDirectionProcessor.instance.EndHighlight();
    }

}
