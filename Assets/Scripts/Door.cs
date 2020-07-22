using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject fractured;
    public bool locked;
    public float breachForce;

    public void Interact()
    {
        if (!locked)
        {
            //Open, Close
        }
    }

    public void ExplosionBreach(float force, Vector3 explosionPos, float explosionRadius, float initialForce)
    {
        if (force >= breachForce)
        {
            fractured.SetActive(true);
            Destroy(gameObject);
        }
    }
}
