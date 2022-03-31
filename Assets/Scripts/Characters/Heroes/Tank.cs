using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Hero
{
    public override int MoveRange { get => moveRange + HeroStatistics.TankMoveRangeBonus; }

    public override float GetAttackDamage()
    {
        return damage + HeroStatistics.TankDamageBonus;
    }
    public override int GetAttackRange()
    {
        return attackRange + HeroStatistics.TankAttackRangeBonus;
    }
    public override string getStats()
    {
        return $"HP: {health}/{maxHealth + HeroStatistics.TeamHealthBonus}\nMovementRange: {moveRange}\nShoot-Range: {attackRange}\nBurst-Cooldown: {HeroManager.instance.TankBurstCooldown}/{HeroManager.instance.TankBurstMaxCooldown}\nBlock-Cooldown: {HeroManager.instance.TankBlockCooldown}/{HeroManager.instance.TankBlockMaxCooldown}";
    }
}
