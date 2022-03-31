using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerArmorUpgrade : Upgrade
{
    [SerializeField]
    float armorAmount;
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerArmorBonus += armorAmount;

    }
}
