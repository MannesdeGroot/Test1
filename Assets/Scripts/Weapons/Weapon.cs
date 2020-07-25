using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats weaponInfo;
    public GameObject muzzleFlash;
    public AudioClip shot;
    public FireMode fireMode;
    [SerializeField] private Transform launchPosition;
    private int ammo;
    private bool reloading;

    private void Start()
    {
        ammo = weaponInfo.clipSize;
        fireMode = weaponInfo.fireMode;
        HUD.SetAmmoText(ammo, weaponInfo.clipSize);
    }

    public void Fire(InfantryController ic, Vector3 recoilValue)
    {
        if (ammo > 0 && !reloading)
        {
            GameObject flash = Instantiate(muzzleFlash, launchPosition.position, transform.rotation);
            AudioSource audioSource = flash.GetComponent<AudioSource>();
            audioSource.clip = shot;
            audioSource.Play();
            Destroy(flash, 3);

            GameObject projectile = Instantiate(weaponInfo.projectile, launchPosition.position, launchPosition.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(launchPosition.forward * weaponInfo.muzzleVelocity);
            projectile.GetComponent<Bullet>().startVelocity = weaponInfo.muzzleVelocity;
            ammo--;
            HUD.SetAmmoText(ammo, weaponInfo.clipSize);
            ic.RecoilShake(recoilValue * weaponInfo.recoilMultiplier);
        }
    }

    public IEnumerator Reload()
    {
        if (!reloading)
        {
            reloading = true;
            //Play anim, sound

            yield return new WaitForSeconds(weaponInfo.reloadTime);

            ammo = weaponInfo.clipSize;
            HUD.SetAmmoText(ammo, weaponInfo.clipSize);
            reloading = false;
        }
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
