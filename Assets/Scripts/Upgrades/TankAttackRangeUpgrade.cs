using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttackRangeUpgrade : Upgrade
{
    [SerializeField]
    int rangeIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.TankAttackRangeBonus += rangeIncrease;
    }

}
