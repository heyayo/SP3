using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EOC States/Transform")]
public class EOCTransform : BossState
{
    [SerializeField]
    private GameObject gorePrefab;

    [SerializeField]
    private Sprite[] gore;

    private float rotatedAmount;

    override public void EnterState()
    {
        rotatedAmount = 0f;
        damageSource.activeEnergyDamage += 50;

        foreach (Sprite sprite in gore)
        {
            dir = new Vector2(Random.Range(-360, 360), Random.Range(-360, 360)).normalized;
            GameObject go = Instantiate(gorePrefab, transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().AddForce(dir * Random.Range(3, 6), ForceMode2D.Impulse);
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            go.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-200, 200));
        }
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
