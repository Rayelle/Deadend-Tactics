using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Hero
{
    public override int MoveRange { get => moveRange + HeroStatistics.MedicMoveRangeBonus; }

    public override float GetAttackDamage()
    {
        return damage + HeroStatistics.MedicDamageBonus;
    }
    public override int GetAttackRange()
    {
        return attackRange + HeroStatistics.MedicAttackRangeBonus;
    }
    public override string getStats()
    {
        return $"HP: {health}/{maxHealth + HeroStatistics.TeamHealthBonus}\nMovementRange: {moveRange}\nShoot-Range: {attackRange}\nHeal-Cooldown: {HeroManager.instance.MedicHealCooldown}/{HeroManager.instance.MedicHealMaxCooldown}\nHeal-Range: {HeroStatistics.MedicHealRange}";
    }
}
