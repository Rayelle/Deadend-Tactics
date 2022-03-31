using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerCritUpgrade : Upgrade
{
    [SerializeField]
    float gunnerCritChanceIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerCritChance += gunnerCritChanceIncrease;
    }
}
