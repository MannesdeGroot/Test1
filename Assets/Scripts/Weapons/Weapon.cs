using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats weaponInfo;
    public FireMode fireMode;
    [SerializeField] private Transform launchPosition;

    private void Start()
    {
        fireMode = weaponInfo.fireMode;
    }

    public void Fire()
    {
        GameObject projectile = Instantiate(weaponInfo.projectile, launchPosition.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * weaponInfo.muzzleVelocity);
    }

    public void Reload()
    {

    }

    public void ToggleFireMode()
    {
        if (weaponInfo.canToggle)
        {
            switch (fireMode)
            {
                case FireMode.FULL_AUTO:
                    fireMode = FireMode.SEMI_AUTO;
                    break;
                case FireMode.SEMI_AUTO:
                    fireMode = FireMode.FULL_AUTO;
                    break;
            }
        }
    }
}
