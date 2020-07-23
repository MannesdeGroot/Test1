using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
public class WeaponStats : ScriptableObject
{
    [Header("Fire Mode")]
    public bool canToggle;
    public FireMode fireMode = FireMode.SEMI_AUTO;

    [Header("Bullet Info")]
    public GameObject projectile;
    public int clipSize;
    public float reloadTime;

    [Header("Firing")]
    public float muzzleVelocity;
    public float roundsPerMinute;
    public float recoilMultiplier;
}
