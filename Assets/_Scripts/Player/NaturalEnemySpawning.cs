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
            _enemyIndex++;
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
            Instantiate(enemies[_enemyIndex], new Vector2((float)x, (float)y), Quaternion.identity);
        }
    }
}
