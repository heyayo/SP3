using System;
using UnityEngine;

public class NaturalEnemySpawning : MonoBehaviour
{
    private float _lastSpawnTime;
    private int _enemyIndex;
    [SerializeField] private float frequency;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int[] count;
    [SerializeField] private double range;
    
    // Start is called before the first frame update
    void Start()
    {
        _lastSpawnTime = Time.time;
        _enemyIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastSpawnTime + frequency < Time.time)
        {
            _lastSpawnTime = Time.time;
            SpawnEnemies();
            _enemyIndex = (_enemyIndex + 1) % enemies.Length;
        }
    }

    [ContextMenu("Spawn Enemies")]
    private void SpawnEnemies()
    {
        for (int i = 0; i < count[_enemyIndex]; ++i)
        {
            double step = (Math.PI * 2.0) / (double)count[_enemyIndex];
            double x = Math.Cos(step * (double)i) * range;
            double y = Math.Sin(step * (double)i) * range;
            Vector2 pos = new Vector2((float)x, (float)y) + (Vector2)transform.position;
            float halfX = WorldGenOptions.worldSize.x / 2;
            float halfY = WorldGenOptions.worldSize.y / 2;
            if (pos.x > halfX) pos.x = halfX-1;
            if (pos.y > halfY) pos.y = halfY-1;
            if (pos.x < -halfX) pos.x = -halfX+1;
            if (pos.y < -halfY) pos.y = -halfY+1;
            Instantiate(enemies[_enemyIndex], pos, Quaternion.identity);
        }
    }
}
