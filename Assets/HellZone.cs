using System.Collections;
using UnityEngine;

public class HellZone : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private bool isStopped = false;
    private Vector2 stopDirection;

    public void Initialize(Vector2 direction, float launchForce, Transform newTarget)
    {
        target = newTarget;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * launchForce;
    }

    private void Update()
    {
        if (target != null)
        {
            if (!isStopped)
            {
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (distanceToTarget <= 5f)
                {
                    rb.velocity = Vector2.zero;
                    isStopped = true;
                    stopDirection = directionToTarget;

                    StartCoroutine(ResumeMovement());
                }
                else
                {
                    rb.velocity = directionToTarget * rb.velocity.magnitude;
                }
            }
        }
    }

    private IEnumerator ResumeMovement()
    {
        yield return new WaitForSeconds(1f);

        if (target != null)
        {
            rb.velocity = stopDirection * rb.velocity.magnitude;
            isStopped = false;
        }
    }
}