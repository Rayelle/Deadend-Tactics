using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerStunGrenadeUpgrade : Upgrade
{
    public static Upgrade instance;


    public override void ApplyUpgrade()
    {
        HeroStatistics.GunnerStunGrenade = true;
    }
}
