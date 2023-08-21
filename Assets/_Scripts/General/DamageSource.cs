using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float hpDamage;
    [SerializeField] private float activeEnergyDamage;
    [Header("Resistance Counter")]
    [SerializeField] private float armourPen;
    [SerializeField] private float resistPen;
    [SerializeField] private Mortality.PierceType armourPenType;
    [SerializeField] private Mortality.PierceType resistPenType;
    [Header("Energy Damage Bleed Over Percentage")]
    [SerializeField] private float bleedOverPercentage;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Damagable"))
        {
            var dmg = other.GetComponent<Damagable>();
            dmg.TakeDamage(gameObject,hpDamage,armourPen,armourPenType,activeEnergyDamage,resistPen,resistPenType,bleedOverPercentage);
        }
    }
}
