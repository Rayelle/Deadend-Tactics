using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathProtectionUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        HeroStatistics.DeathProtection = true;
    }
}
