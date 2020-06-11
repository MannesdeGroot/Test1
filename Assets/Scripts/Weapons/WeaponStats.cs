using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    [Header("Fire Mode")]
    public bool canToggle;
    public FireMode fireMode = FireMode.SEMI_AUTO;

    [Header("Bullet Info")]
    public GameObject projectile;
    public int clipSize;

    [Header("Firing")]
    public float muzzleVelocity;
    public float roundsPerMinute;
}
