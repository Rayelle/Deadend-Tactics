using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMoveRangeUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        HeroStatistics.TankMoveRangeBonus += 1;
    }
}
