using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    static public BossManager Instance;

    public class BossStats 
    {
        public bool bossAlive;
        public bool bossDefeated;
        public BossItem boss;

        // Creating new bossStats
        static public BossStats New(BossItem bossItem)
        {
            BossStats bossStat = new BossStats();
            bossStat.bossAlive = false;
            bossStat.bossDefeated = false;
            bossStat.boss = bossItem;
            return bossStat;
        }

        public void SetAlive(bool bossAlive)
        {
            this.bossAlive = bossAlive;
        }
    }

    [HideInInspector]
    public Dictionary<string, BossStats> bossList = new Dictionary<string, BossStats>();

    private void Start()
    {
        // Global
        Instance = this;
        foreach (BossItem bossItem in Resources.LoadAll<BossItem>("Items/BossSummons/"))
        {
            bossList.Add(bossItem.bossName, BossStats.New(bossItem));
        }
    }

    public bool SummonBoss(string summonedBossName)
    {
        // Only one of each boss can be spawned at once
        if (bossList[summonedBossName].bossAlive)
            return false;

        bossList[summonedBossName].SetAlive(true);

        Transform playerTransform = PlayerManager.Instance.transform;

        // Spawn Randomly around player
        float angle = Random.Range(0f, 360f);
        float x = 20f * Mathf.Cos(angle);
        float y = 20f * Mathf.Sin(angle);
        Vector2 spawnPos = new Vector2(x, y);

        GameObject summoned = Instantiate(bossList[summonedBossName].boss.bossPrefab, new Vector2(playerTransform.position.x, playerTransform.position.y)
            + spawnPos, Quaternion.identity);
        summoned.GetComponent<BossFSM>().bossName = summonedBossName;

        return true;
    }

    public void KillBoss(string summonedBossName)
    {
        if (!bossList[summonedBossName].bossAlive)
            return;

        bossList[summonedBossName].bossAlive = false;
        bossList[summonedBossName].bossDefeated = true;
        Debug.Log("SHOULD");

        // If no more bosses alive then change the bgm back to normal
        foreach (BossItem bossItem in Resources.LoadAll<BossItem>("Items/BossSummons/"))
        {
            Debug.Log(bossItem.bossName + ": " + bossList[bossItem.bossName].bossAlive);

            if (bossList[bossItem.bossName].bossAlive)
                break;

            SoundManager.Instance.PlayBGM(0);
        }
    }
}
