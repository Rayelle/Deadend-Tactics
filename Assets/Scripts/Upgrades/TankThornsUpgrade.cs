using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankThornsUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        HeroStatistics.TankThornsUpgrade = true;
    }

}
