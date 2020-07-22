using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Infantry", menuName = "Controllers/Infantry")]
public class InfantryStats : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
}
