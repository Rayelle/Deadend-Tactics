using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicAttackRangeUpgrade : Upgrade
{
    [SerializeField]
    int rangeIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.MedicAttackRangeBonus += rangeIncrease;
    }
}
