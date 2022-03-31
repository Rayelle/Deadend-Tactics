using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Hero
{
    public override int MoveRange { get => moveRange+HeroStatistics.GunnerMoveRangeBonus; }

    protected override void Start()
    {
        health = maxHealth;
        GameEvents.instance.onDestroyUnit += CheckForKill;
    }
    private void OnDisable()
    {
        GameEvents.instance.onDestroyUnit -= CheckForKill;
    }
    /// <summary>
    /// check for kills to update autocrit
    /// </summary>
    /// <param name="deadUnit"></param>
    private void CheckForKill(Unit deadUnit)
    {
        //if a unit was killed by this hero set autocritcooldown to true
        if(HeroManager.instance.SelectedHero!=null)
            if(HeroManager.instance.SelectedHero.MyUnitType == HeroEnums.UnitType.gunner 
                && HeroStatistics.GunnerAutoCritUpgrade 
                && deadUnit.MyUnitType == HeroEnums.UnitType.enemy)
            {
                HeroStatistics.GunnerAutoCritOnCooldown = true;
            }
    }
    /// <summary>
    /// get current damage
    /// gunner has a chance for critical damage
    /// </summary>
    /// <returns></returns>
    public override float GetAttackDamage()
    {
        //gunner has a chance to randomly deal critical damage
        if (HeroStatistics.GunnerAutoCritUpgrade)
        {
            if (HeroStatistics.GunnerAutoCritOnCooldown)
            {
                HeroStatistics.GunnerAutoCritOnCooldown = false;
                return damage * HeroStatistics.GunnerCritMultiplier + HeroStatistics.GunnerDamageBonus;
            }
        }
        System.Random rnd = new System.Random();
        if ( rnd.NextDouble() <= HeroStatistics.GunnerCritChance)
        {
            return damage * HeroStatistics.GunnerCritMultiplier + HeroStatistics.GunnerDamageBonus;
        }
        return damage;
    }
    public override int GetAttackRange()
    {
        return attackRange + HeroStatistics.GunnerAttackRangeBonus;
    }
    public override string getStats()
    {
        return $"HP: {health}/{maxHealth+HeroStatistics.TeamHealthBonus}\nMovementRange: {moveRange}\nShoot-Range: {attackRange}\nGrenade-Cooldown: {HeroManager.instance.GunnerGrenadeCooldown}/{HeroManager.instance.GunnerGrenadeMaxCooldown}\nExplosion-Range: {HeroStatistics.GunnerGrenadeExplosionRange}";
    }
}
