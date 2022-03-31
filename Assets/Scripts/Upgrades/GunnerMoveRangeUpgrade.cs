using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerMoveRangeUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerMoveRangeBonus += 1;
    }
}
