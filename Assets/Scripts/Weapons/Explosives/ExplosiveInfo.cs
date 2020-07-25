using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveInfo : ScriptableObject
{
    public float blastForce;
    public float shockForce;
    public float blastRadius;
    public float shockRadius;
    public bool timed;
    public float time;
}