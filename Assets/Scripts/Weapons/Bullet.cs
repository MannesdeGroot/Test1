using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Caliber caliber;
    private AudioSource audioSource;
    private Rigidbody rb;
    private bool penetrated = true;
    private float startVelocity;
    private float velocityOnHit;
    private float distancePenetrated;

    private float traceCountdown = 0.5f;
    private float traceTimer;
    private List<Vector3> bulletPath = new List<Vector3>();

    private void Start()
    {
        bulletPath.Add(transform.position);
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = caliber.clip;
        audioSource.Play();
    }

    private void Update()
    {
        if(startVelocity == 0)
            startVelocity = Mathf.Abs((transform.rotation * rb.velocity).x);

        CheckForWall();

        if (rb.velocity != Vector3.zero)
        {
            traceTimer -= Time.deltaTime * 5;
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
        else if (!bulletPath.Contains(transform.position))
            bulletPath.Add(transform.position);
    }

    void CheckForWall()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.02f) && penetrated)
        {
            penetrated = false;
            velocityOnHit = Mathf.Abs((transform.rotation * rb.velocity).x);
            rb.velocity *= 0;
            Penetrate(hit.collider);
        }
    }

    private void Penetrate(Collider col)
    {
        Penetratable otherObject = col.GetComponent<Penetratable>();

        if (otherObject != null)
        {
            float normalizedVelocity = velocityOnHit / startVelocity;

            if (!Physics.CheckSphere(transform.position + transform.forward * PenetrationDepth(otherObject.density, normalizedVelocity), 0.01f))
            {
                Vector3 penetratePos = transform.position + transform.forward * PenetrationDepth(otherObject.density, normalizedVelocity);

                if (col.Raycast(new Ray(penetratePos, -transform.forward), out RaycastHit hit, Mathf.Infinity))
                {
                    penetrated = true;
                    distancePenetrated = Vector3.Distance(transform.position, hit.point);
                    transform.position = hit.point;
                    rb.velocity = transform.forward * CalculateVelocity();
                }
            }
            else
                rb.useGravity = false;
        }
    }

    private float PenetrationDepth(float density, float normalizedVelocity)
    {
        return caliber.length * (caliber.density / density) * normalizedVelocity;
    }

    private float CalculateVelocity()
    {
        return velocityOnHit - velocityOnHit * (caliber.mass / distancePenetrated);
    }

    private void OnCollisionEnter(Collision c)
    {
        bulletPath.Add(transform.position);
        CheckForWall();

        Health health = c.transform.GetComponent<Health>();
        if (health != null)
        {
            health.ChangeHealth(0);
        }

        bulletPath.Add(transform.position);
        //Destroy(gameObject);
    }
}
