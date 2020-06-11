using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryController : MonoBehaviour
{
    [SerializeField] private InfantryStats stats;

    [Header("Look")]
    private Camera cam;
    [SerializeField] private Transform head, eyes;
    [SerializeField] private float snapBackSpeed;
    [SerializeField] private float lookUpClamp, lookSideClamp;
    [SerializeField] private float normalFov, runFov;
    [SerializeField] private float fovChangeSpeed;
    private Vector3 eyeRot, headRot, bodyRot;
    private bool freeLook;

    [Header("Weapons")]
    public Transform gunPos;
    public GameObject currentWeapon;
    private Weapon weapon;
    private float fireTimer;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        weapon = currentWeapon.GetComponent<Weapon>();
    }

    private void Update()
    {
        Move();
        Look();
        Fire();
        Aim();
    }

    private void Move()
    {
        Vector3 move = new Vector3();

        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");

        transform.Translate(move * Time.deltaTime * Speed());

        if (Input.GetButton("Shift"))
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runFov, fovChangeSpeed * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFov, fovChangeSpeed * Time.deltaTime);
        }
    }

    private void Look()
    {
        eyeRot.x -= Input.GetAxis("Mouse Y") * Time.deltaTime * Settings.sensitivity;
        eyeRot.x = Mathf.Clamp(eyeRot.x, -lookUpClamp, lookUpClamp);
        eyes.localRotation = Quaternion.Euler(eyeRot);

        if (Input.GetButtonDown("Left Alt"))
            headRot = new Vector3(0, bodyRot.y, 0);

        if (Input.GetButton("Left Alt"))
        {
            headRot.y += Input.GetAxis("Mouse X") * Time.deltaTime * Settings.sensitivity;
            headRot.y = Mathf.Clamp(headRot.y, bodyRot.y - lookSideClamp, bodyRot.y + lookSideClamp);
            head.rotation = Quaternion.Euler(headRot);
            freeLook = true;
        }
        else
        {
            bodyRot.y += Input.GetAxis("Mouse X") * Time.deltaTime * Settings.sensitivity;
            transform.rotation = Quaternion.Euler(bodyRot);

            if (head.rotation.y != transform.rotation.y)
            {
                head.rotation = Quaternion.Lerp(head.rotation, transform.rotation, snapBackSpeed * Time.deltaTime);
                return;
            }

            if (freeLook)
            {
                print("a");
                if (eyes.rotation != currentWeapon.transform.rotation)
                {
                    print("b");
                    eyes.rotation = Quaternion.Lerp(eyes.rotation, currentWeapon.transform.rotation, snapBackSpeed * Time.deltaTime);
                    print($"{eyes.rotation},{currentWeapon.transform.rotation}");
                    return;
                }
                else freeLook = false;
            }
            else
            {
                currentWeapon.transform.position = gunPos.position;
                currentWeapon.transform.rotation = gunPos.rotation;
            }
        }
    }

    private float Speed()
    {
        if (Input.GetButton("Shift")) return stats.runSpeed;
        else if (Input.GetButton("Left CTRL")) return stats.crouchSpeed;
        else return stats.walkSpeed;
    }

    private void Fire()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer > 0) return;

        if (weapon.weaponInfo.fireMode == WeaponStats.mode.SEMI_AUTO)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.Fire();
                fireTimer = weapon.weaponInfo.fireRate / 3600f;
                print(fireTimer);
            }
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                weapon.Fire();
                fireTimer = weapon.weaponInfo.fireRate / 60f;
                print(fireTimer);
            }
        }
    }

    private void Aim()
    {

    }
}
