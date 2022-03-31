using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamArmorUpgrade : Upgrade
{
    [SerializeField]
    float armorIncrease = 1.0f;
    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerArmorBonus += armorIncrease;
        HeroStatistics.TankArmorBonus += armorIncrease;
        HeroStatistics.MedicArmorBonus += armorIncrease;
    }

}
