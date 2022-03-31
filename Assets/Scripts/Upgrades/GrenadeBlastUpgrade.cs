using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBlastUpgrade : Upgrade
{
    [SerializeField]
    int grenadeBlastRangeBoost;
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerGrenadeExplosionRange += grenadeBlastRangeBoost;
    }
}
