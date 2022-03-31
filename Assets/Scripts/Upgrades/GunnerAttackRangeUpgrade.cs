using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAttackRangeUpgrade : Upgrade
{
    [SerializeField]
    int rangeIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerAttackRangeBonus += rangeIncrease;
    }
}
