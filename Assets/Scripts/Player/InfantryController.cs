using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryController : MonoBehaviour
{
    [SerializeField] private InfantryStats stats;
    private Animator anim;

    [Header("Look")]
    private Camera cam;
    [SerializeField] private Transform spine, head, eyes;
    [SerializeField] private float snapBackSpeed;
    [SerializeField] private float lookUpClamp, lookSideClamp;
    [SerializeField] private float normalFov, runFov;
    [SerializeField] private float fovChangeSpeed;
    public Vector3 eyeRot, headRot, bodyRot;
    public float verticalInput;
    public bool freeLook;
    [Space()]
    [Space()]
    [SerializeField] private float recoilRotationSpeed;
    [SerializeField] private float recoilReturnSpeed;
    public Vector3 recoilRotationHipFire;
    public Vector3 recoilRotationAiming;
    private Vector3 shakeRotAmplifier, recoilRot;

    [Header("Weapons")]
    public Transform gunPos;
    public GameObject currentWeapon;
    private Weapon weapon;
    private float fireTimer;
    [SerializeField] private float weaponRotationSpeed;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        weapon = currentWeapon.GetComponent<Weapon>();
        anim = GetComponentInChildren<Animator>();
        currentWeapon.transform.SetParent(null);
    }

    private void Update()
    {
        Move();
        Fire();
        MoveWeapon();
        Look();
        Aim();
        Reload();
    }

    private void LateUpdate()
    {
        if (freeLook)
        {
            if (eyes.rotation != currentWeapon.transform.rotation)
            {
                eyeRot = Quaternion.Lerp(eyes.rotation, currentWeapon.transform.rotation, snapBackSpeed * Time.deltaTime).eulerAngles;
                eyes.rotation = Quaternion.Euler(eyeRot.x, headRot.y, 0);
            }
            else
            {
                verticalInput = 0;
                freeLook = false;
            }
        }
        else
        {
            spine.localRotation = Quaternion.Euler(eyeRot.x, headRot.y, 0);
            eyes.rotation = head.rotation;
        }
    }

    private void Move()
    {
        Vector3 move = new Vector3();

        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");

        transform.Translate(move * Time.deltaTime * Speed());

        anim.SetFloat("Side", move.x * Speed());
        anim.SetFloat("ForwardSpeed", move.z * Speed());

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
        verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * Settings.sensitivity;
        eyeRot.x -= verticalInput;
        eyeRot.x = Mathf.Clamp(eyeRot.x, -lookUpClamp, lookUpClamp);

        if (Input.GetButtonDown("Left Alt"))
        {
            headRot = new Vector3(0, bodyRot.y, 0);
            freeLook = true;
        }

        if (Input.GetButton("Left Alt"))
        {
            headRot.y += Input.GetAxis("Mouse X") * Time.deltaTime * Settings.sensitivity;
            headRot.y = Mathf.Clamp(headRot.y, bodyRot.y - lookSideClamp, bodyRot.y + lookSideClamp);
            head.rotation = Quaternion.Euler(headRot);
            eyes.localRotation = Quaternion.Euler(eyeRot.x, headRot.y, 0);
        }
        else
        {
            bodyRot.y += Input.GetAxis("Mouse X") * Time.deltaTime * Settings.sensitivity;
            transform.rotation = Quaternion.Euler(bodyRot);
            if (head.rotation.y != transform.rotation.y)
            {
                head.rotation = Quaternion.Lerp(head.rotation, transform.rotation, snapBackSpeed * Time.deltaTime);
            }
        }

        shakeRotAmplifier = Vector3.Lerp(shakeRotAmplifier, eyeRot, recoilReturnSpeed * Time.deltaTime);
        recoilRot = Vector3.Slerp(recoilRot, shakeRotAmplifier, recoilRotationSpeed * Time.deltaTime);
        eyes.localRotation = Quaternion.Euler(eyes.rotation.eulerAngles + shakeRotAmplifier);
    }

    private void MoveWeapon()
    {
        currentWeapon.transform.position = gunPos.position;
        currentWeapon.transform.rotation = Quaternion.Slerp(currentWeapon.transform.rotation, gunPos.rotation, weaponRotationSpeed / weapon.weaponInfo.weight * Time.deltaTime);
    }

    private float Speed()
    {
        if (Input.GetButton("Shift")) return stats.runSpeed;
        else if (Input.GetButton("Left CTRL")) return stats.crouchSpeed;
        else return stats.walkSpeed;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire3")) weapon.ToggleFireMode();

        fireTimer -= Time.deltaTime;

        if (fireTimer < 0)
        {
            // aiming?

            if (weapon.fireMode == FireMode.SEMI_AUTO)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    weapon.Fire(this, recoilRotationHipFire); //Aim of Hip?
                    fireTimer = 60 / weapon.weaponInfo.roundsPerMinute;
                }
            }
            else
            {
                if (Input.GetButton("Fire1"))
                {
                    weapon.Fire(this, recoilRotationHipFire); //Aim of Hip?
                    fireTimer = 60 / weapon.weaponInfo.roundsPerMinute;
                }
            }
        }
    }

    private void Reload()
    {
        if (Input.GetButtonDown("Reload"))
            StartCoroutine(weapon.Reload());
    }

    private void Aim()
    {

    }

    public void RecoilShake(Vector3 value)
    {
        float recoilY = Random.Range(-value.y, value.y);
        float recoilZ = Random.Range(-value.z, value.z);
        shakeRotAmplifier += new Vector3(value.x, recoilY, recoilZ);
    }
}