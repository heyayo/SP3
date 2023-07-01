using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Mono.Cecil;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] WorldGenOptions options;
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Sprite[] groundTiles;
    [SerializeField] private Tilemap environmentMap;
    [SerializeField] private Sprite[] environmentTiles;

    [SerializeField] private string seed = "BREENSEERAYYANG";
    [SerializeField] private float seedFloat = 0f;
    
    // Possibly Internalized Later
    [SerializeField] private float scale = 0.1f;

    private float _InternalEnvironmentSeedOffset = 423554f;

    private Tile CreateTile(Sprite sprite, Color color)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = color;
        Debug.Log("Loaded Tile | " + sprite.name);
        return tile;
    }

    public void GenerateWorld()
    {
        ConvertSeed();
        
        // Adjust Environment Tilemap
        GameObject a = new GameObject();
        a.transform.LookAt(Camera.main.transform);
        environmentMap.orientationMatrix = Matrix4x4.Rotate(a.transform.rotation);

        // Ground Tiles
        Tile grassTile = CreateTile(groundTiles[0],new Color(0,0.4f,0));
        Tile waterTile = CreateTile(groundTiles[1], Color.cyan);
        
        // Environment Tiles
        Tile treeTile = CreateTile(environmentTiles[0], Color.green);
        
        for (int x = -options.worldSize.x/2; x < options.worldSize.x/2; ++x)
        {
            for (int y = -options.worldSize.y/2; y < options.worldSize.y/2; ++y)
            {
                float groundLevel = Mathf.PerlinNoise(x * scale + seedFloat, y * scale + seedFloat);
                if (groundLevel > options.seaLevel)
                    groundMap.SetTile(new Vector3Int(x,y,0), grassTile);
                else
                    groundMap.SetTile(new Vector3Int(x,y,0), waterTile);
                
                float environmentLevel = Mathf.PerlinNoise(x * scale + _InternalEnvironmentSeedOffset + seedFloat,
                    y * scale + _InternalEnvironmentSeedOffset + seedFloat);

                if (groundLevel <= options.seaLevel) continue;
                if (environmentLevel > options.treeLevel) environmentMap.SetTile(new Vector3Int(x,y,0),treeTile);
            }
        }
        
        groundMap.RefreshAllTiles();
    }

    public void ConvertSeed()
    {
        if (seed.Length <= 0)
        {
            seedFloat = Random.value * Int32.MaxValue;
        }
        seedFloat = 0f;
        List<float> debugList = new List<float>();
        foreach (var c in seed)
        {
            seedFloat += c;
            debugList.Add(c);
        }

        string debugListResult = "";
        foreach (var f in debugList)
        {
            debugListResult += f.ToString() + " ";
        }
        
        Debug.Log(debugListResult);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }
}
