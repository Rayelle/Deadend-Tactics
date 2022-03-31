using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDamageUpgrade : Upgrade
{
    [SerializeField]
    float damageIncrease;
    public override void ApplyUpgrade()
    {
        HeroStatistics.TankDamageBonus += damageIncrease;
    }

}
