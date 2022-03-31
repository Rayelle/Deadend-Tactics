using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankArmorUpgrade : Upgrade
{
    [SerializeField]
    float armorAmount;
    public override void ApplyUpgrade()
    {
        HeroStatistics.TankArmorBonus += armorAmount;

    }

}
