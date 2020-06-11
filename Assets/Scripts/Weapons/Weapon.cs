using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats weaponInfo;
    [SerializeField] private Transform launchPosition;

    public void Fire()
    {
        GameObject projectile = Instantiate(weaponInfo.projectile, launchPosition.position, launchPosition.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(launchPosition.forward * weaponInfo.muzzleVelocity);
    }

    public void Reload()
    {

    }
}
