using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResistanceStats : MonoBehaviour
{
    [Header("Player Script")]
    [SerializeField] private Mortality mortality;

    [SerializeField] private TMP_Text armourStat;
    [SerializeField] private TMP_Text resistStat;

    // Start is called before the first frame update
    void Start()
    {
        mortality = PlayerManager.Instance.MortalityScript;

        UpdateArmourStat();
        UpdateResistStat();
        
        mortality.onArmourAdjust.AddListener(UpdateArmourStat);
        mortality.onResistAdjust.AddListener(UpdateResistStat);
        var inveSlots = InventoryManager.Instance.inventorySlots;
        var armSlots = InventoryManager.Instance.armourSlots;
    }

    private void UpdateArmourStat()
    { armourStat.text = mortality.Armour.ToString(); }
    private void UpdateResistStat()
    { resistStat.text = mortality.Resist.ToString(); }
}
