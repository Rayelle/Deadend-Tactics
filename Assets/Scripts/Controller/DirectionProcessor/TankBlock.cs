using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBlock : MonoBehaviour
{
    public static TankBlock instance;
    Hero myHero;
    private bool blockActive=false;
    [SerializeField]
    GameObject shieldAnimation;
    public Hero MyHero { set => myHero = value; }

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
        GameEvents.instance.onEndAITurn += RemoveArmor;
        GameEvents.instance.onSpawnUnit += CheckforTank;
    }
    private void OnDisable()
    {
        GameEvents.instance.onEndAITurn -= RemoveArmor;
        GameEvents.instance.onSpawnUnit -= CheckforTank;
    }
    /// <summary>
    /// called by end-ai-turn-event
    /// deactivates tank-invincibility
    /// </summary>
    private void RemoveArmor()
    {
        if (blockActive)
        {
            HeroStatistics.TankArmorBonus -= 100.0f;
            //animate going back to normal
            blockActive = false;
        }
    }
    /// <summary>
    /// Registers tank when hero is spawned.
    /// </summary>
    /// <param name="toCheck"></param>
    private void CheckforTank(Unit toCheck)
    {
        if (toCheck.MyUnitType == HeroEnums.UnitType.tank)
        {
            myHero = toCheck.GetComponent<Hero>();
        }
    }
    /// <summary>
    /// Activates tank-invincibility.
    /// </summary>
    public void ActivateTankBlock()
    {
        //instantiate shield animation
        GameObject.Instantiate(shieldAnimation, IsoGrid.instance.ToWorldSpace(myHero.gridPosition), Quaternion.identity);
        if (!blockActive && myHero!=null)
        {
            //tank becomes invincible
            HeroStatistics.TankArmorBonus += 100.0f;
            blockActive = true;

            //if taunt upgrade is gathered, enemies surrounding the tank will become taunted
            if(HeroStatistics.TankTauntUpgrade)
                foreach (Vector2Int spaceInTauntRange in MapContent.instance.getSpacesInRange(myHero.gridPosition,HeroStatistics.TankTauntRange))
                {
                    if (MapContent.instance.SpaceContainsEnemy(spaceInTauntRange))
                    {
                        MapContent.instance.Dictionary[spaceInTauntRange].GetComponent<ZombieStateMachine>().ChangeState(TauntedBehaviour.instance);
                    }
                }
        }
    }
}
