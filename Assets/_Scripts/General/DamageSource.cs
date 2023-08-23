using UnityEngine;
using UnityEngine.Events;

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
    
    public delegate Affliction[] Afflicter();
    public Afflicter afflicter = null;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Damagable"))
        {
            var dmg = other.GetComponent<Damagable>();
            dmg.TakeDamage(gameObject,hpDamage,armourPen,armourPenType,activeEnergyDamage,resistPen,resistPenType,bleedOverPercentage);
            if (afflicter != null)
            {
                var afflictions = afflicter();
                dmg.TakeAfflictions(afflictions);
            }
        }
    }
}
