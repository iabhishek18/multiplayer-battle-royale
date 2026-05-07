using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeaponStats
{
    public string id;
    public string displayName;
    public WeaponType type;
    public float damage;
    public float fireRate;
    public float range;
    public float reloadTime;
    public int magazineSize;
    public float bulletSpeed;
    public float spreadAngle;
    public float headshotMultiplier;
    public float falloffStartRange;
    public float falloffEndRange;
    public float falloffMinDamage;
    public Rarity rarity;
}

public enum WeaponType { AssaultRifle, SMG, Shotgun, Sniper, Pistol, LMG }
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }

public static class WeaponDatabase
{
    private static readonly Dictionary<string, WeaponStats> weapons = new()
    {
        ["ar_standard"] = new WeaponStats { id = "ar_standard", displayName = "M4A1", type = WeaponType.AssaultRifle, damage = 32, fireRate = 7.5f, range = 120, reloadTime = 2.1f, magazineSize = 30, bulletSpeed = 900, spreadAngle = 1.2f, headshotMultiplier = 2.0f, falloffStartRange = 60, falloffEndRange = 100, falloffMinDamage = 18, rarity = Rarity.Common },
        ["smg_fast"] = new WeaponStats { id = "smg_fast", displayName = "MP5", type = WeaponType.SMG, damage = 22, fireRate = 12f, range = 50, reloadTime = 1.8f, magazineSize = 35, bulletSpeed = 700, spreadAngle = 2.5f, headshotMultiplier = 1.8f, falloffStartRange = 25, falloffEndRange = 45, falloffMinDamage = 12, rarity = Rarity.Common },
        ["shotgun_pump"] = new WeaponStats { id = "shotgun_pump", displayName = "Pump Shotgun", type = WeaponType.Shotgun, damage = 95, fireRate = 1.0f, range = 15, reloadTime = 4.5f, magazineSize = 5, bulletSpeed = 400, spreadAngle = 8f, headshotMultiplier = 2.5f, falloffStartRange = 5, falloffEndRange = 12, falloffMinDamage = 25, rarity = Rarity.Uncommon },
        ["sniper_bolt"] = new WeaponStats { id = "sniper_bolt", displayName = "AWP", type = WeaponType.Sniper, damage = 110, fireRate = 0.6f, range = 300, reloadTime = 3.5f, magazineSize = 5, bulletSpeed = 1200, spreadAngle = 0.1f, headshotMultiplier = 3.0f, falloffStartRange = 200, falloffEndRange = 280, falloffMinDamage = 70, rarity = Rarity.Epic },
    };

    public static WeaponStats GetWeapon(string id) => weapons.GetValueOrDefault(id);

    public static float CalculateDamage(WeaponStats weapon, float distance, bool isHeadshot)
    {
        float damage = weapon.damage;

        if (distance > weapon.falloffStartRange)
        {
            float falloffProgress = Mathf.Clamp01((distance - weapon.falloffStartRange) / (weapon.falloffEndRange - weapon.falloffStartRange));
            damage = Mathf.Lerp(weapon.damage, weapon.falloffMinDamage, falloffProgress);
        }

        if (isHeadshot) damage *= weapon.headshotMultiplier;
        return Mathf.Round(damage);
    }

    public static float GetDPS(WeaponStats weapon) => weapon.damage * weapon.fireRate;
    public static float GetTTK(WeaponStats weapon, int targetHP = 100) => (targetHP / weapon.damage) / weapon.fireRate;
}
