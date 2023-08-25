using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Header("Damage")]
    public float hpDamage;
    public float activeEnergyDamage;
    [SerializeField] private float nativeHPDamage;
    [SerializeField] private float nativeActiveEnergyDamage;
    [Header("Resistance Counter")]
    public float armourPen;
    public float resistPen;
    [SerializeField] private float nativeArmourPen;
    [SerializeField] private float nativeResistPen;
    public Mortality.PierceType armourPenType;
    public Mortality.PierceType resistPenType;
    [Header("Energy Damage Bleed Over Percentage")]
    public float bleedOverPercentage;
    [SerializeField] private float nativeBleedOverPercentage;

    [Header("Friendly Fire Fix")]
    public bool isFriendly = false;
    public bool isHostile = true;

    public float __NativeHPDamage
    {
        get => nativeHPDamage;
        set
        {
            float delta = value - nativeHPDamage;
            hpDamage += delta;
            nativeHPDamage = value;
        }
    }

    public float __NativeActiveEnergyDamage
    {
        get => nativeActiveEnergyDamage;
        set
        {
            float delta = value - nativeActiveEnergyDamage;
            activeEnergyDamage += delta;
            nativeActiveEnergyDamage = value;
        }
    }

    public float __NativeArmourPen
    {
        get => nativeArmourPen;
        set
        {
            float delta = value - nativeArmourPen;
            armourPen += delta;
            nativeArmourPen = value;
        }
    }

    public float __NativeResistPen
    {
        get => nativeResistPen;
        set
        {
            float delta = value - nativeResistPen;
            resistPen += delta;
            nativeResistPen = value;
        }
    }

    public float __NativeBleedOverPercentage
    {
        get => nativeBleedOverPercentage;
        set
        {
            float delta = value - nativeBleedOverPercentage;
            bleedOverPercentage += delta;
            nativeBleedOverPercentage = value;
        }
    }
    
    public delegate Affliction[] Afflicter();
    public Afflicter afflicter = null;

    private void Start()
    {
        hpDamage = __NativeHPDamage;
        activeEnergyDamage = __NativeActiveEnergyDamage;
        armourPen = __NativeArmourPen;
        resistPen = __NativeResistPen;
        bleedOverPercentage = __NativeBleedOverPercentage;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Damagable"))
        {
            var player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                if (isFriendly)
                    return;
            }
            else
            {
                if (isHostile)
                    return;
            }
            var dmg = other.GetComponent<Damagable>();
            dmg.TakeDamage(gameObject,hpDamage,armourPen,armourPenType,activeEnergyDamage,resistPen,resistPenType,bleedOverPercentage);
            if (afflicter != null)
            {
                var afflictions = afflicter();
                dmg.TakeAfflictions(afflictions);
            }
        }
    }

    [ContextMenu("DEBUG_AddDamage")]
    private void DEBUG_AddDamage()
    {
        __NativeActiveEnergyDamage += 100;
        __NativeHPDamage += 20;
    }
}
