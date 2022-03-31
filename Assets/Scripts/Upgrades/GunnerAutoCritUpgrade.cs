using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAutoCritUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerAutoCritUpgrade = true;
        HeroStatistics.GunnerAutoCritOnCooldown = false;
    }

}
