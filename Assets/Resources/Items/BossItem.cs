using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Boss Item")]
public class BossItem : Item
{
    [Header("Spawns this boss")]
    [SerializeField] public string bossName;
    [SerializeField]
    public GameObject bossPrefab;

    public UnityEvent summon;

    private void OnEnable()
    {
        if (summon == null)
            summon = new UnityEvent();
    }

    public override void Use()
    {
        Transform playerTransform = PlayerManager.Instance.transform;

        // Spawn Randomly around player
        float angle = Random.Range(0f, 360f);
        float x = 20f * Mathf.Cos(angle);
        float y = 20f * Mathf.Sin(angle);
        Vector2 spawnPos = new Vector2(x, y);

        /*
        Instantiate(bossPrefab, new Vector2(playerTransform.position.x, playerTransform.position.y)
            + spawnPos, Quaternion.identity);
         */
        
        consumed.Invoke();
        if (BossManager.Instance.SummonBoss(bossName))
            consumed.Invoke();
    }
}
