using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerDamageUpgrade : Upgrade
{
    [SerializeField]
    float damageIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerDamageBonus += damageIncrease;
    }
}
