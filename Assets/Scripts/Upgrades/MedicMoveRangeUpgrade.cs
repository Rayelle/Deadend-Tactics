using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicMoveRangeUpgrade : Upgrade
{
    [SerializeField]
    int moveRangeIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerArmorBonus += moveRangeIncrease;

    }

}
