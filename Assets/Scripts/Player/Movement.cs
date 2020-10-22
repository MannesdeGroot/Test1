using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public InfantryStats playerStats;
    public Weapon weapon;
    public Transform weaponPos;

    [Header("Movement")]
    private Vector3 move;
    private float speed;

    [Header("Look")]
    private Vector3 backRot, headRot, headRotH, hRotVector;
    public Transform back, head, cam;
    public float verticalClamp;
    public float freelookSideClamp;
    private float vRot, hRot;

    private void Start()
    {
        speed = playerStats.walkSpeed;
    }

    private void Update()
    {
        Move();
        Look();
        WeaponUsage();
    }

    private void LateUpdate()
    {
        RotateBones();
        cam.position = head.position;
        cam.rotation = head.rotation;
    }

    private void Move()
    {
        move.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        move.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(move);
    }

    private void Look()
    {
        hRot = Input.GetAxis("Mouse X") * Settings.sensitivity * Time.deltaTime;
        vRot = -Input.GetAxis("Mouse Y") * Settings.sensitivity * Time.deltaTime;

        if (Input.GetButtonDown("Left Alt"))
        {
            headRot = new Vector3(head.rotation.x, 0, 0);
        }

        if (!Input.GetButton("Left Alt"))
        {
            transform.Rotate(0, hRot, 0);
            backRot += new Vector3(vRot, 0, 0);
            backRot.x = Mathf.Clamp(backRot.x, -verticalClamp, verticalClamp);

            if (head.localRotation != transform.rotation)
            {
                headRot = Vector3.Slerp(headRot, Vector3.zero, 10 * Time.deltaTime);
                headRotH = Vector3.Slerp(headRotH, Vector3.zero, 10 * Time.deltaTime);
            }
        }
        else
        {
            headRotH += new Vector3(0, hRot, 0);
            headRotH.y = Mathf.Clamp(headRotH.y, transform.rotation.y - freelookSideClamp, transform.rotation.y + freelookSideClamp);
            headRot += new Vector3(vRot, 0, 0);
            headRot.x = Mathf.Clamp(headRot.x, -verticalClamp, verticalClamp);
        }
    }

    private void RotateBones()
    {
        back.localRotation = Quaternion.Euler(backRot);
        head.localRotation = Quaternion.Euler(headRot.x, headRotH.y, 0);
        weapon.transform.position = weaponPos.position;
        weapon.transform.rotation = weaponPos.rotation;
    }

    private void WeaponUsage()
    {
        if(weapon.fireMode == FireMode.FULL_AUTO)
        {
            if (Input.GetButton("Fire1"))
            {
                weapon.Fire();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.Fire();
            }
        }

        if (Input.GetButtonDown("Reload"))
        {
            StartCoroutine(weapon.Reload());
        }

        if (Input.GetButtonDown("Fire3"))
        {
            weapon.ToggleFireMode();
        }
    }
}
