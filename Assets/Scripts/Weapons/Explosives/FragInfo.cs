using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosive", menuName = "Weapons/Explosive/Frag")]
public class FragInfo : ExplosiveInfo
{
    public int fragPieces;
    public float fragRange;
}
