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
        GameObject projectile = Instantiate(weaponInfo.projectile, launchPosition.position, launchPosition.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(launchPosition.forward * weaponInfo.muzzleVelocity);

        if(Physics.Raycast(launchPosition.position, launchPosition.forward, out RaycastHit hit))
        {
            Debug.DrawRay(launchPosition.position, launchPosition.forward * 100, Color.red, 5f);
            print(hit.transform.name);
        }
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
        print(fireMode);
    }
}
