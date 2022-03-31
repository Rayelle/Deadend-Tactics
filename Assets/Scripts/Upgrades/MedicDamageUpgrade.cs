using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicDamageUpgrade : Upgrade
{
    [SerializeField]
    float damageIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.MedicDamageBonus += damageIncrease;
    }

}
