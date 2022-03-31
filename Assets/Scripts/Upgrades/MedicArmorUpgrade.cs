using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicArmorUpgrade : Upgrade
{
    [SerializeField]
    float armorAmount;
    public override void ApplyUpgrade()
    {
        HeroStatistics.MedicArmorBonus += armorAmount;
    }
}
