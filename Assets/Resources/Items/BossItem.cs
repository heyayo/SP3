using UnityEngine;

[CreateAssetMenu(menuName = "Items/Boss Item")]
public class BossItem : Item
{
    [Header("Spawns this boss")]
    [SerializeField] private AltBossManager.Bosses BossID;
    [SerializeField] public string bossName;

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
        
        AltBossManager.Instance.Summon(BossID,
            new Vector2(playerTransform.position.x, playerTransform.position.y) + spawnPos);

        consumed.Invoke();
    }
}