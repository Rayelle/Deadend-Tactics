using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHealthUpgrade : Upgrade
{
    //public static Upgrade instance;

    [SerializeField]
    float healthBoost = 6;

    public override void ApplyUpgrade()
    {
        HeroStatistics.TeamHealthBonus += healthBoost;
    }
}
