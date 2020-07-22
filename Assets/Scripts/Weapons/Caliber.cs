using UnityEngine;

[CreateAssetMenu(fileName = "Caliber", menuName = "Weapons/Caliber")]
public class Caliber : ScriptableObject
{
    public float mass;
    public float length;
    public float density;
    public AudioClip clip;
}
