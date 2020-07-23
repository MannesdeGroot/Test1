
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public ExplosiveInfo explosive;
    public GameObject explosion;
    float timer;

    private void Start()
    {
        timer = explosive.time;
    }

    private void Update()
    {
        if (explosive.timed)
        {
            timer -= Time.deltaTime;
            if (timer < 0) Explode();
        }
        else
        {
            if (Input.GetButtonDown("Jump")) Explode();
        }
    }

    protected virtual void Explode()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);

        Collider[] collidersDestroy = Physics.OverlapSphere(transform.position, explosive.blastRadius);
        foreach (Collider col in collidersDestroy)
        {
            Door door = col.GetComponent<Door>();
            if (door != null)
            {
                door.ExplosionBreach(CalculateExplosionForce(transform.position, door.transform.position), transform.position, explosive.blastRadius, explosive.force);
            }
        }

        Collider[] collidersForce = Physics.OverlapSphere(transform.position, explosive.blastRadius);
        foreach (Collider col in collidersForce)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosive.force, transform.position, explosive.blastRadius);
            }
        }

        Destroy(gameObject);
    }

    private float CalculateExplosionForce(Vector3 source, Vector3 targetPos)
    {
        return explosive.force / Vector3.Distance(source, targetPos);
    }
}
