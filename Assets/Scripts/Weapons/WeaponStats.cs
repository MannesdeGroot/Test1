using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    [Header("Fire Mode")]
    public bool canToggle;
    public enum mode { SEMI_AUTO, FULL_AUTO }
    public mode fireMode = mode.SEMI_AUTO;

    [Header("Bullet Info")]
    public GameObject projectile;
    public int clipSize;

    [Header("Firing")]
    public float muzzleVelocity;
    public float fireRate;
}
