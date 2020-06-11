using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InfantryStats : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
}
