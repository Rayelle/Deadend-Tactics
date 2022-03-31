using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    HealthBar myHealthBar;
    public HealthBar MyHealthBar { get => myHealthBar; set => myHealthBar = value; }
    private float restHealAmount=8;
    /// <summary>
    /// take damage based on unit-type
    /// </summary>
    /// <param name="finalAmount"></param>
    public override void TakeDamage(float finalAmount)
    {
        switch (MyUnitType)
        {

            case HeroEnums.UnitType.gunner:
                TakeDamageWithModifiers(finalAmount, HeroStatistics.GunnerArmorBonus);
                break;
            case HeroEnums.UnitType.tank:
                TakeDamageWithModifiers(finalAmount, HeroStatistics.TankArmorBonus);
                if (HeroStatistics.TankThornsUpgrade)
                {
                    ApplyTankThorns();
                }
                break;
            case HeroEnums.UnitType.medic:
                TakeDamageWithModifiers(finalAmount, HeroStatistics.MedicArmorBonus);
                break;

        }

        if (myHealthBar!=null)
            myHealthBar.SetHealth(Health);
    }

    /// <summary>
    /// reduce health by given amount
    /// modified by armor
    /// </summary>
    /// <param name="finalAmount"></param>
    /// <param name="armorBonus"></param>
    /// <param name="armorMultiplier"></param>
    private void TakeDamageWithModifiers(float finalAmount, float armorBonus)
    {
        if (health - (finalAmount - (armor+armorBonus)) <= 0.0f)
        {
            health = 0;
            Die();
        }
        else
        {
            AnimationHelper.instance.FlashImageRed(mySpriteRenderer);
            health -= (finalAmount - (armor + armorBonus));
        }
    }
    /// <summary>
    /// get healed by given amount
    /// </summary>
    /// <param name="amount"></param>
    public override void GetHealed(float amount)
    {
        if (health + amount >= maxHealth + HeroStatistics.TeamHealthBonus)
        {
            health = maxHealth;
            if (myHealthBar != null)
                myHealthBar.SetHealth(Health);
        }
        else
        {
            health += amount;
            if (myHealthBar != null)
                myHealthBar.SetHealth(Health);
        }

    }

    /// <summary>
    /// regenerate health by 10%
    /// </summary>
    public void RestBetweenRounds()
    {
        mySpriteRenderer.enabled = true;
        float healAmount = restHealAmount;
        GetHealed(healAmount);
    }
    /// <summary>
    /// reset units health
    /// </summary>
    public override void ResetUnit()
    {
        health = maxHealth + HeroStatistics.TeamHealthBonus ;
    }
    /// <summary>
    /// remove hero from map and disable its sprite
    /// </summary>
    public override void Die()
    {
        if (HeroStatistics.DeathProtection)
        {
            HeroStatistics.DeathProtection = false;
            GetHealed(maxHealth * HeroStatistics.DeathProtectionMaxHPMultiplier);
        }
        else
        {
            AnimationHelper.instance.CreateSmokeCloud(IsoGrid.instance.ToWorldSpace(gridPosition));
            MapContent.instance.DeleteAt(myCurrentPosition);
            mySpriteRenderer.enabled = false;
            HeroManager.instance.HeroDied();
        }
    }
    /// <summary>
    /// Apply damage towards tiles around the tanks position
    /// </summary>
    private void ApplyTankThorns()
    {
        foreach (Vector2Int space in MapContent.instance.getSpacesInRange(HeroManager.instance.Tank.gridPosition,1))
        {
            if (MapContent.instance.SpaceContainsEnemy(space))
            {
                MapContent.instance.Dictionary[space].TakeDamage(HeroStatistics.TankThornsDamage);
            }
        }
    }
}
