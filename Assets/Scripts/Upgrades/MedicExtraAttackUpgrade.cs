using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicExtraAttackUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        HeroStatistics.MedicExtraAttackUpgrade = true;
    }

}
