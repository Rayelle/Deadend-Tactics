using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTauntUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        HeroStatistics.TankTauntUpgrade = true;
    }

}
