using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// contains static hero values which can be altered through upgrades
/// </summary>
public static class HeroStatistics
{
    
    private static int gunnerAttackRangeBonus = 0,gunnerMoveRangeBonus = 0, gunnerGrenadeRange = 5, gunnerGrenadeExplosionRange = 2;
    private static int tankAttackRangeBonus = 0, tankMoveRangeBonus = 0, tankTauntRange=3, tankThornsDamage=5;
    private static int medicAttackRangeBonus = 0, medicMoveRangeBonus = 0, medicHealRange = 3;
    private static float gunnerDamageBonus=0.0f, gunnerGrenadeDamage = 8, gunnerArmorBonus = 0.0f, gunnerCritChance=0.2f, gunnerCritMultiplier=1.5f;
    private static float teamHealthBonus = 0.0f;
    private static float tankDamageBonus = 0.0f, tankBurstDamage = 5, tankArmorBonus = 0.0f;
    private static float medicDamageBonus = 0.0f, medicArmorBonus = 0.0f, medicHealAmout = 7;
    private static bool deathProtection = false, gunnerStunGrenade = false, medicExtraAttackUpgrade = false, medicExtraAttackOnCooldown=true, tankTauntUpgrade=false, gunnerAutoCritUpgrade=false, gunnerAutoCritOnCooldown=false,tankThornsUpgrade=false;
    private static float deathProtectionHeal = 0.2f; //how much percent of maxHp is healed by deathProtection
    public static int GunnerGrenadeRange { get => gunnerGrenadeRange; set => gunnerGrenadeRange = value; }
    public static int GunnerGrenadeExplosionRange { get => gunnerGrenadeExplosionRange; set => gunnerGrenadeExplosionRange = value; }
    public static float GunnerGrenadeDamage { get => gunnerGrenadeDamage; set => gunnerGrenadeDamage = value; }
    public static float GunnerArmorBonus { get => gunnerArmorBonus; set => gunnerArmorBonus = value; }
    public static float TankArmorBonus { get => tankArmorBonus; set => tankArmorBonus = value; }
    public static float MedicArmorBonus { get => medicArmorBonus; set => medicArmorBonus = value; }
    public static int MedicHealRange { get => medicHealRange; set => medicHealRange = value; }
    public static float MedicHealAmout { get => medicHealAmout; set => medicHealAmout = value; }
    public static float TeamHealthBonus { get => teamHealthBonus; set => teamHealthBonus = value; }
    public static float TankBurstDamage { get => tankBurstDamage; set => tankBurstDamage = value; }
    public static float GunnerCritChance { get => gunnerCritChance; set => gunnerCritChance = value; }
    public static float GunnerCritMultiplier { get => gunnerCritMultiplier; set => gunnerCritMultiplier = value; }
    public static float TankDamageBonus { get => tankDamageBonus; set => tankDamageBonus = value; }
    public static float GunnerDamageBonus { get => gunnerDamageBonus; set => gunnerDamageBonus = value; }
    public static float MedicDamageBonus { get => medicDamageBonus; set => medicDamageBonus = value; }
    public static int GunnerAttackRangeBonus { get => gunnerAttackRangeBonus; set => gunnerAttackRangeBonus = value; }
    public static int TankAttackRangeBonus { get => tankAttackRangeBonus; set => tankAttackRangeBonus = value; }
    public static int MedicAttackRangeBonus { get => medicAttackRangeBonus; set => medicAttackRangeBonus = value; }
    public static int GunnerMoveRangeBonus { get => gunnerMoveRangeBonus; set => gunnerMoveRangeBonus = value; }
    public static int TankMoveRangeBonus { get => tankMoveRangeBonus; set => tankMoveRangeBonus = value; }
    public static int MedicMoveRangeBonus { get => medicMoveRangeBonus; set => medicMoveRangeBonus = value; }
    public static bool DeathProtection { get => deathProtection; set => deathProtection = value; }
    public static float DeathProtectionMaxHPMultiplier { get => deathProtectionHeal; set => deathProtectionHeal = value; }
    public static bool GunnerStunGrenade { get => gunnerStunGrenade; set => gunnerStunGrenade = value; }
    public static bool MedicExtraAttackOnCooldown { get => medicExtraAttackOnCooldown; set => medicExtraAttackOnCooldown = value; }
    public static bool MedicExtraAttackUpgrade { get => medicExtraAttackUpgrade; set => medicExtraAttackUpgrade = value; }
    public static int TankTauntRange { get => tankTauntRange; set => tankTauntRange = value; }
    public static bool TankTauntUpgrade { get => tankTauntUpgrade; set => tankTauntUpgrade = value; }
    public static bool GunnerAutoCritOnCooldown { get => gunnerAutoCritOnCooldown; set => gunnerAutoCritOnCooldown = value; }
    public static bool GunnerAutoCritUpgrade { get => gunnerAutoCritUpgrade; set => gunnerAutoCritUpgrade = value; }
    public static bool TankThornsUpgrade { get => tankThornsUpgrade; set => tankThornsUpgrade = value; }
    public static int TankThornsDamage { get => tankThornsDamage; set => tankThornsDamage = value; }

}
