using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInputProcessor : InputProcessor
{
    public static AttackInputProcessor instance;
    [SerializeField]
    GameObject rangeIndicatorPrefab;

    private List<GameObject> allRangeHighlights;
    private List<Vector2Int> positionsInRange;

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
    /// start up attack-phase
    /// </summary>
    public override void Init()
    {
        InputManager.instance.CurrentInputProcessor = AttackInputProcessor.instance;
        InputManager.instance.CurrentDirectionInputProcessor = SpaceSelectorDirectionProcessor.instance;

        //spawn highlights inside the attack-range
        positionsInRange = ZombieHelper.GetSpacesInRange(HeroManager.instance.SelectedHero.gridPosition, HeroManager.instance.SelectedHero.GetAttackRange());
        allRangeHighlights = TileHelper.instance.SpawnHighlightsAround(positionsInRange, rangeIndicatorPrefab, this.transform);
    }

   

    /// <summary>
    /// Accept currently selected space. If space is occupied by an enemy, an attack is made.
    /// </summary>
    public override void Accept()
    {
        //if a valid tile was selected, execute attack with hero
        if (positionsInRange.Contains(SpaceSelectorDirectionProcessor.instance.HighlightPos))
        {
            if (MapContent.instance.SpaceContainsEnemy(SpaceSelectorDirectionProcessor.instance.HighlightPos))
            {

                StartCoroutine(ExecuteAttack());
                return;
            }
        }
        //selected an illegal target for this attack
        AudioManager.instance.PlayMenuErrorSound();
    }
    /// <summary>
    /// plays attack animation and soundeffect, deals damage to target
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExecuteAttack()
    {
        InputManager.instance.DeactivatePlayerControl();
        //plas attack sound after a delay
        StartCoroutine(PlayAttackSoundAfterDelay());
        if(HeroManager.instance.SelectedHero.MyAnimator!=null)
            HeroManager.instance.SelectedHero.MyAnimator.SetBool("attacking", true);
        yield return new WaitForSeconds(HeroManager.instance.SelectedHero.AttackDuration);
        InputManager.instance.ActivatePlayerControl();
        if (HeroManager.instance.SelectedHero.MyAnimator != null)
            HeroManager.instance.SelectedHero.MyAnimator.SetBool("attacking", false);
        //medic deals poison damage, other heroes do not
        if (HeroManager.instance.SelectedHero.MyUnitType == HeroEnums.UnitType.medic)
        {
            MapContent.instance.Dictionary[SpaceSelectorDirectionProcessor.instance.HighlightPos].TakePoisonDamage(HeroManager.instance.SelectedHero.GetAttackDamage());
            if (HeroStatistics.MedicExtraAttackUpgrade)
            {
                if (HeroStatistics.MedicExtraAttackOnCooldown)
                {
                    //medic can use attack a second time if the upgrade is active
                    HeroStatistics.MedicExtraAttackOnCooldown = false;
                }
                else
                {
                    HeroStatistics.MedicExtraAttackOnCooldown = true;
                    EndAttackPhase();
                }
            }
            else
            {
                EndAttackPhase();
            }
        }
        else
        {
            if(MapContent.instance.SpaceContainsEnemy(SpaceSelectorDirectionProcessor.instance.HighlightPos))
                MapContent.instance.Dictionary[SpaceSelectorDirectionProcessor.instance.HighlightPos].TakeDamage(HeroManager.instance.SelectedHero.GetAttackDamage());
            EndAttackPhase();
        }
    }

    private IEnumerator PlayAttackSoundAfterDelay()
    {
        if (HeroManager.instance.SelectedHero.AttackAudio != null)
        {
            yield return new WaitForSeconds(HeroManager.instance.SelectedHero.AttackSoundEffectDelay);
            HeroManager.instance.SelectedHero.AttackAudio.Play();
        }
    }
    /// <summary>
    /// stop attacking
    /// </summary>
    public override void Refuse()
    {
        EndAttackPhase();
    }
    /// <summary>
    /// Destroy all gameObjects of range-highlight
    /// </summary>
    private void EndAttackPhase()
    {
        foreach (GameObject current in allRangeHighlights)
        {
            Destroy(current);
        }
        HeroSelectionInputProcessor.instance.Init();
        GameEvents.instance.EndActivation(HeroManager.instance.SelectedHero);
    }
}
