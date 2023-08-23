using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Boss Item")]
public class BossItem : Item
{
    [Header("Spawns this boss")]
    [SerializeField]
    public GameObject bossPrefab;

    [SerializeField]
    public string bossName;

    public UnityEvent summon;

    private void OnEnable()
    {
        if (summon == null)
            summon = new UnityEvent();
    }

    public override void Use()
    {
        if (BossManager.Instance.SummonBoss(bossName))
            consumed.Invoke();
    }
}