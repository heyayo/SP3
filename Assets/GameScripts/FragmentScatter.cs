using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FragmentScatter : MonoBehaviour
{
    [SerializeField] private GameObject fragmentPrefab;
    [SerializeField] private Sprite[] fragmentSprites;
    [SerializeField] private List<GameObject> fragments;
    
    private WorldGenOptions _options;
    private WorldGenerator _generator;

    private void Awake()
    {
        _options = WorldGenOptions.FetchConfig();
    }

    private void Start()
    {
        _generator = GetComponent<WorldGenerator>();
        _generator.onWorldEndGen.AddListener(SpawnFragments);
    }

    // Returns an array of positions where fragments can be scattered
    public Vector2Int[] ScatterFragments()
    {
        var positions = new Vector2Int[8];
        for (int i = 0; i < 8; ++i)
        {
            float x = (Random.value-0.5f) * _options.worldSize.x;
            float y = (Random.value-0.5f) * _options.worldSize.y;
            positions[i] = new Vector2Int((int)x, (int)y);
        }

        return positions;
    }

    [ContextMenu("Spawn Fragments")]
    public void SpawnFragments()
    {
        foreach (var fragment in fragments)
            Destroy(fragment);
        fragments.Clear();
        var positions = ScatterFragments();
        foreach (var pos in positions)
        {
            GameObject a = Instantiate(fragmentPrefab, (Vector2)pos, quaternion.identity);
            fragments.Add(a);
        }
    }
}
