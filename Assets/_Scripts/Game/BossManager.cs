using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance { get; private set; }
    private PlayerManager _player;

    [SerializeField] private GameObject[] bossPrefabs;

    private GameObject[] activeBosses;

    public enum Bosses
    {
        EOC,
        BOC,
        Plantera,
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Boss Managers In Scene");
            Debug.Break();
        }
        Instance = this;
    }

    private void Start()
    {
        _player = PlayerManager.Instance;
    }

    public void BossDeath(Bosses bossID)
    {
        Destroy(activeBosses[(int)bossID]);
        activeBosses[(int)bossID] = null;
    }

    public void Summon(Bosses bossID, Vector2 position = new Vector2())
    {
        var boss = activeBosses[(int)bossID];
        if (boss != null)
        {
            Debug.Log(boss.name + " Already Spawned");
            return;
        }

        activeBosses[(int)bossID] = Instantiate(bossPrefabs[(int)bossID], _player.transform.position, Quaternion.identity);
    }
}
