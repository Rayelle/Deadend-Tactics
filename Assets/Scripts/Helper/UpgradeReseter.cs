using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this class is responsible for setting all the values in static HeroStats class each time the user returns to the main menu
public class UpgradeReseter : MonoBehaviour
{
    private int gunnerAttackRangeBonus = 0, gunnerMoveRangeBonus = 0, gunnerGrenadeRange = 5, gunnerGrenadeExplosionRange = 2;
    private int tankAttackRangeBonus = 0, tankMoveRangeBonus = 0, tankTauntRange = 3, tankThornsDamage = 5;
    private int medicAttackRangeBonus = 0, medicMoveRangeBonus = 0, medicHealRange = 3;
    private float gunnerDamageBonus = 0.0f, gunnerGrenadeDamage = 8, gunnerArmorBonus = 0.0f, gunnerCritChance = 0.2f, gunnerCritMultiplier = 1.5f;
    private float teamHealthBonus = 0.0f;
    private float tankDamageBonus = 0.0f, tankBurstDamage = 10, tankArmorBonus = 0.0f;
    private float medicDamageBonus = 0.0f, medicArmorBonus = 0.0f, medicHealAmout = 6;
    private bool deathProtection = false, gunnerStunGrenade = false, medicExtraAttackUpgrade = false, medicExtraAttackOnCooldown = true, tankTauntUpgrade = false, gunnerAutoCritUpgrade = false, gunnerAutoCritOnCooldown = false, tankThornsUpgrade = false;
    private float deathProtectionHeal = 0.2f; //how much percent of maxHp is healed by deathProtection
    // Start is called before the first frame update
    void Start()
    {
        ResetAllUpgradeValues();
    }
    private void ResetAllUpgradeValues()
    {
        HeroStatistics.GunnerAttackRangeBonus = gunnerAttackRangeBonus;
        HeroStatistics.GunnerMoveRangeBonus = gunnerMoveRangeBonus;
        HeroStatistics.GunnerGrenadeRange = gunnerGrenadeRange;
        HeroStatistics.GunnerGrenadeExplosionRange = gunnerGrenadeExplosionRange;
        HeroStatistics.TankAttackRangeBonus = tankAttackRangeBonus;
        HeroStatistics.TankMoveRangeBonus = tankMoveRangeBonus;
        HeroStatistics.TankTauntRange = tankTauntRange;
        HeroStatistics.TankThornsDamage = tankThornsDamage;
        HeroStatistics.MedicAttackRangeBonus = medicAttackRangeBonus;
        HeroStatistics.MedicMoveRangeBonus = medicMoveRangeBonus;
        HeroStatistics.MedicHealRange = medicHealRange;
        HeroStatistics.GunnerDamageBonus = gunnerDamageBonus;
        HeroStatistics.GunnerGrenadeDamage = gunnerGrenadeDamage;
        HeroStatistics.GunnerArmorBonus = gunnerArmorBonus;
        HeroStatistics.GunnerCritChance = gunnerCritChance;
        HeroStatistics.GunnerCritMultiplier = gunnerCritMultiplier;
        HeroStatistics.TeamHealthBonus = teamHealthBonus;
        HeroStatistics.TankDamageBonus = tankDamageBonus;
        HeroStatistics.TankBurstDamage = tankBurstDamage;
        HeroStatistics.TankArmorBonus = tankArmorBonus;
        HeroStatistics.MedicDamageBonus = medicDamageBonus;
        HeroStatistics.MedicArmorBonus = medicArmorBonus;
        HeroStatistics.MedicHealAmout = medicHealAmout;
        HeroStatistics.DeathProtection = deathProtection;
        HeroStatistics.GunnerStunGrenade = gunnerStunGrenade;
        HeroStatistics.MedicExtraAttackUpgrade = medicExtraAttackUpgrade;
        HeroStatistics.MedicExtraAttackOnCooldown = medicExtraAttackOnCooldown;
        HeroStatistics.TankTauntUpgrade = tankTauntUpgrade;
        HeroStatistics.GunnerAutoCritUpgrade = gunnerAutoCritUpgrade;
        HeroStatistics.GunnerAutoCritOnCooldown = gunnerAutoCritOnCooldown;
        HeroStatistics.TankThornsUpgrade = tankThornsUpgrade;
        HeroStatistics.DeathProtectionMaxHPMultiplier = deathProtectionHeal;
    }
}
