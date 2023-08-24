using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plantera States/Transform")]
public class PlanteraTransform : BossState
{
    [SerializeField]
    private GameObject gorePrefab;

    [SerializeField]
    private Sprite[] gore;

    public override void EnterState()
    {
        Vector2 dir;

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
        animator.SetBool("Enraged", true);
        SoundManager.Instance.PlaySound(3);
        return true;
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
