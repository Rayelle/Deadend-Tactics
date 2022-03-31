using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEnums;

namespace HeroEnums
{
    public enum HeroActions
    {
        noSelection,
        wait,
        shoot,
        frag,
        block,
        heal,
        burst
    }
    public enum UnitType
    {
        obstacle,
        enemy,
        gunner,
        tank,
        medic
    }
}

public class HeroManager : MonoBehaviour
{
    public static HeroManager instance;
    [SerializeField]
    GameObject looseMenu;
    private List<Unit> allHeroes = new List<Unit>();
    private Unit tank;

    private Unit selectedHero;
    private uint gunnerGrenadeCooldown = 0, tankBurstCooldown = 0, tankBlockCooldown = 0, medicHealCooldown = 0;
    [SerializeField]
    private uint gunnerGrenadeMaxCooldown, tankBurstMaxCooldown, tankBlockMaxCooldown, medicHealMaxCooldown;
    [SerializeField]
    float heroMoveDelay = 0.2f;

    public List<Unit> AllHeroes { get => allHeroes; }
    public Unit SelectedHero { get => selectedHero; set => selectedHero = value; }
    public Unit Tank { get => tank; }
    public uint GunnerGrenadeCooldown { get => gunnerGrenadeCooldown; set => gunnerGrenadeCooldown = value; }
    public uint TankBurstCooldown { get => tankBurstCooldown; set => tankBurstCooldown = value; }
    public uint TankBlockCooldown { get => tankBlockCooldown; set => tankBlockCooldown = value; }
    public uint MedicHealCooldown { get => medicHealCooldown; set => medicHealCooldown = value; }
    public uint GunnerGrenadeMaxCooldown { get => gunnerGrenadeMaxCooldown; }
    public uint TankBurstMaxCooldown { get => tankBurstMaxCooldown; }
    public uint TankBlockMaxCooldown { get => tankBlockMaxCooldown; }
    public uint MedicHealMaxCooldown { get => medicHealMaxCooldown; }
    public float HeroMoveDelay { get => heroMoveDelay; }

    private float eventWaitTime = 0.1f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameEvents.instance.onSpawnUnit += RegisterNewHero;
        GameEvents.instance.onEndActivation += HeroDone;
        GameEvents.instance.onEndAITurn += ReReadyHeroes;
    }

    private void OnDisable()
    {
        GameEvents.instance.onSpawnUnit -= RegisterNewHero;
        GameEvents.instance.onEndActivation -= HeroDone;
        GameEvents.instance.onEndAITurn -= ReReadyHeroes;
    }
    /// <summary>
    /// HeroManager keeps track of all heroes
    /// </summary>
    /// <param name="unit"></param>
    private void RegisterNewHero(Unit unit)
    {
        if (unit.MyUnitType == UnitType.medic || unit.MyUnitType == UnitType.tank || unit.MyUnitType == UnitType.gunner)
        {
            if (!allHeroes.Contains(unit))
            {
                allHeroes.Add(unit);
                unit.Ready = true;
                if (unit.MyUnitType == UnitType.tank)
                {
                    tank = unit;
                }
            }
        }
    }
    /// <summary>
    /// check if all heroes are dead and then end the game
    /// </summary>
    public void HeroDied()
    {
        bool allDead = true;
        foreach (Unit currentHero in allHeroes)
        {
            if (currentHero.Health <= 0.0f)
            {
                allDead = false;
            }
        }
        if (allDead)
        {
            looseMenu.SetActive(true);
        }
    }
    /// <summary>
    /// Rereadys hero and changes current selection
    /// if all heroes moved end the turn
    /// </summary>
    /// <param name="activatedUnit"></param>
    private void HeroDone(Unit activatedUnit)
    {
        activatedUnit.Ready = false;
        selectedHero = null;
        bool allHeroesDone = true;
        foreach (Unit currentHero in allHeroes)
        {
            if (currentHero.Ready)
            {
                allHeroesDone = false;
                break;
            }
        }

        if (allHeroesDone)
        {
            StartCoroutine(EndPlayerTurnAfterDelay());
        }

    }
    /// <summary>
    /// reduce cooldowns of heroes and set them to be ready again
    /// </summary>
    public void ReReadyHeroes()
    {
        foreach (Unit hero in allHeroes)
        {
            if (hero.Health > 0 && !hero.Stunned)
            {
                hero.Ready = true;
            }
        }
        if (gunnerGrenadeCooldown > 0)
        {
            gunnerGrenadeCooldown--;
        }
        if (tankBurstCooldown > 0)
        {
            tankBurstCooldown--;
        }
        if (tankBlockCooldown > 0)
        {
            tankBlockCooldown--;
        }
        if (medicHealCooldown > 0)
        {
            medicHealCooldown--;
        }
    }
    /// <summary>
    /// end turn after slight delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndPlayerTurnAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GameEvents.instance.EndPlayerTurn();
    }
}
