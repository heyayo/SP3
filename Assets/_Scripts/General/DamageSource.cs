using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Header("Damage")]
    public float hpDamage;
    public float activeEnergyDamage;
    [Header("Resistance Counter")]
    public float armourPen;
    public float resistPen;
    public Mortality.PierceType armourPenType;
    public Mortality.PierceType resistPenType;
    [Header("Energy Damage Bleed Over Percentage")]
    public float bleedOverPercentage;
    private void Start()
    {
        bleedOverPercentage = 0f;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Damagable"))
        {
            Debug.Log(gameObject + "IS GETTING HIT");
            var dmg = other.GetComponent<Damagable>();
            dmg.TakeDamage(gameObject,hpDamage,armourPen,armourPenType,activeEnergyDamage,resistPen,resistPenType,bleedOverPercentage);
        }
    }
}
