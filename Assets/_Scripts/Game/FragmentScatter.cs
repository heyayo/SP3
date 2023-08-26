using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FragmentScatter : MonoBehaviour
{
    [SerializeField] private PickupItem fragmentPrefab;
    [SerializeField] private Sprite[] fragmentSprites;

    private GameObject _shenronSummon;
    
    private WorldGenerator _generator;

    private void Start()
    {
        _generator = GetComponent<WorldGenerator>();
        _generator.onWorldEndGen.AddListener(SpawnFragments);
    }

    // Returns an array of positions where fragments can be scattered
    public Vector2Int[] ScatterFragments()
    {
        var positions = new Vector2Int[7];
        for (int i = 0; i < 7; ++i)
        {
            float x = (Random.value-0.5f) * WorldGenOptions.worldSize.x;
            float y = (Random.value-0.5f) * WorldGenOptions.worldSize.y;
            positions[i] = new Vector2Int((int)x, (int)y);
        }

        return positions;
    }

    [ContextMenu("Spawn Fragments")]
    public void SpawnFragments()
    {
        var positions = ScatterFragments();
        foreach (var pos in positions)
        {
            PickupItem a = Instantiate(fragmentPrefab, (Vector2)pos, quaternion.identity);
            a.item.Setup(a.gameObject);
        }
    }
}
