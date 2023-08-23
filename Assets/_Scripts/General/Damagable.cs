using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Mortality))]
public class Damagable : MonoBehaviour
{
    [SerializeField] private Mortality mortality;
    [SerializeField] private float immunityTime = 1;
    [SerializeField] private bool immune;

    private void Awake()
    {
        mortality = GetComponent<Mortality>();
    }

    /*
     * hpDamage | Damage dealt to Health
     * eDamage | Damage dealt to Active Energy
     * ap | Armour Penetration
     * mp | Energy Penetration
     * hpPierce | Armour Piercing
     * ePierce | Resist Piercing
     * bleed | Energy Damage BleedOver to Health, Percentage
     */
    public void TakeDamage(GameObject dealer, float hpDamage, float ap, Mortality.PierceType hpPierce, float eDamage, float mp, Mortality.PierceType ePierce, float bleed)
    {
        if (immune) return;
        mortality.ApplyEnergyDamage(eDamage,mp,ePierce,bleed,ap,hpPierce);
        mortality.ApplyHealthDamage(hpDamage,ap,hpPierce);
        WaitImmunity();
    }

    public void TakeAfflictions(Affliction[] afflictions)
    {
        foreach (var af in afflictions)
            mortality.ApplyAffliction(af);
    }

    private async Task WaitImmunity()
    {
        immune = true;
        int immuneTime = (int)(immunityTime * 1000f);
        await Task.Delay(immuneTime);
        immune = false;
    }

    public static void LogDamage(GameObject target,GameObject dealer,float hpDamage,float ap,float eDamage,float mp,Mortality.PierceType hpPierce,Mortality.PierceType ePierce,float bleed)
    {
        StringBuilder a = new StringBuilder();
        a.Append("DAMAGE LOG | TARGET: ");
        a.Append(target.name);
        a.Append(" | DEALER: ");
        a.Append(dealer.name);
        a.Append(" | HPDAMAGE: ");
        a.Append(hpDamage.ToString());
        a.Append(" | ARMOURPIERCING: ");
        a.Append(ap.ToString());
        a.Append(" | ENERGYDAMAGE: ");
        a.Append(eDamage.ToString());
        a.Append(" RESISTPIERCE: ");
        a.Append(mp.ToString());
        a.Append(" PIERCETYPES,HP,RP: ");
        a.Append(hpPierce.ToString());
        a.Append(" ");
        a.Append(ePierce.ToString());
        a.Append(" | BleedOver: ");
        a.Append(bleed.ToString());
        Debug.Log(a.ToString());
    }
}
