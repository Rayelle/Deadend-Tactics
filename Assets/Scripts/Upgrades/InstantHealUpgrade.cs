using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantHealUpgrade : Upgrade
{
    [SerializeField]
    float healAmount;
    public override void ApplyUpgrade()
    {
        foreach (Unit hero in HeroManager.instance.AllHeroes)
        {
            hero.GetHealed(healAmount);
        }
    }

}
