using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/Transform")]
public class EOCTransform : BossState
{
    private float rotatedAmount;

    override public void EnterState()
    {
        rotatedAmount = 0f;
    }

    override public bool DoState()
    {
        float rotationSpeed = 5f;
        float rotationAmount = 500f;

        // Slow > fast rotation
        if (rotatedAmount < rotationAmount)
            rb.AddTorque(rotationSpeed);

        rotatedAmount += rotationSpeed;

        if (rotatedAmount >= rotationAmount && rb.angularVelocity <= 20f)
        {
            animator.SetBool("Enraged", true);
            SoundManager.Instance.PlaySound(0);
            return true;
        }

        return false;
    }

    override public void ExitState()
    {

    }

    public override bool isReadyToTransform()
    {
        return mortality.Health <= mortality.__HealthMax / 2;
    }

    //private void HitboxDamage()
    //{
    //    // Hitbox stats
    //    Vector2 hitboxPos = transform.position - new Vector3(0, 0.5f, 0);
    //    float hitboxRadius = 1.2f;
    //    Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
    //    if (col != null)
    //        playerMortality.ApplyHealthDamage(10);
    //}
}
