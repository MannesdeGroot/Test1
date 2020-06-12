using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Caliber { SMALL, MEDIUM, HIGH }
    public Caliber caliber = Caliber.HIGH;
    [SerializeField] private float penetrationMultiplier;
    private Rigidbody rb;
    private bool penetrated;
    private float velocityOnHit;

    private float traceCountdown = 0.5f;
    private float traceTimer;
    private List<Vector3> bulletPath = new List<Vector3>();

    /*
     * TODO:
     * - Calculate Velocity after penetration.
     * - Penetration Multiplier tweaking.
     */

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        penetrated = true;
        rb.velocity = new Vector3(rb.velocity.z, rb.velocity.y, rb.velocity.x);
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.02f) && penetrated)
        {
            penetrated = false;
            velocityOnHit = (transform.rotation * rb.velocity).x;
            rb.velocity *= 0;
            Penetrate();
        }

        Debug.DrawRay(transform.position, transform.forward * 0.5f, Color.blue, 0.5f);

        traceTimer -= Time.deltaTime;
        if (traceTimer < 0)
        {
            traceTimer = traceCountdown;
            bulletPath.Add(transform.position);
        }

        if (bulletPath.Count < 2) return;
        for (int i = 0; i < bulletPath.Count - 1; i++)
        {
            Debug.DrawLine(bulletPath[i], bulletPath[i + 1], Color.red, 100);
        }
    }

    private void Penetrate()
    {
        //print(PenetrationDepth());
        if (!Physics.CheckSphere(transform.position + transform.forward * PenetrationDepth(), 0.01f))
        {
            rb.useGravity = false;
            transform.position += transform.forward * PenetrationDepth();
            penetrated = true;

            if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit))
            {
                transform.position = hit.point;
                rb.velocity = transform.forward * CalculateVelocity();
            }
        }
    }

    private float PenetrationDepth()
    {
        return penetrationMultiplier * velocityOnHit * CaliberMultiplier();
    }

    private float CalculateVelocity()
    {
        return 100;
    }

    private float CaliberMultiplier()
    {
        switch (caliber)
        {
            case Caliber.HIGH:
                return 1.5f;
            case Caliber.MEDIUM:
                return 1f;
            case Caliber.SMALL:
                return 0.5f;
        }
        return 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position + transform.forward * PenetrationDepth(), 0.02f);
    }
}
