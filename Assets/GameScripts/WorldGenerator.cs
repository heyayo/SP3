using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    [Header("Options Scriptable Object")]
    [SerializeField] WorldGenOptions options;
    
    [Header("Hardcoded Tile Resources")]
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Sprite[] groundTiles;
    [SerializeField] private Tilemap environmentMap;
    [SerializeField] private Sprite[] environmentTiles;

    [Header("For Debug Visual Only")]
    [SerializeField] private string seed = "BREENSEERAYYANG";
    [SerializeField] private float seedFloat = 0f;
    
    // Possibly Internalized Later
    [Header("Might be Hard Coded in Script Later")]
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
        
        // Ground Tiles
        Tile grassTile = CreateTile(groundTiles[0],new Color(0,0.4f,0));
        Tile waterTile = CreateTile(groundTiles[1], Color.cyan);
        
        // Environment Tiles
        Tile treeTile = CreateTile(environmentTiles[0], Color.green);
        
        for (int x = -options.worldSize.x/2; x < options.worldSize.x/2; ++x)
        {
            for (int y = -options.worldSize.y/2; y < options.worldSize.y/2; ++y)
            {
                float xJustified = (x + seedFloat) * scale;
                float yJustified = (y + seedFloat) * scale;
                
                // Determine Ground Tile Level
                float groundLevel = Mathf.PerlinNoise(xJustified, yJustified);
                if (groundLevel > options.seaLevel)
                    groundMap.SetTile(new Vector3Int(x,y,0), grassTile);
                else
                    groundMap.SetTile(new Vector3Int(x,y,0), waterTile);
                
                if (groundLevel <= options.seaLevel) continue;
                if (groundLevel > options.treeLevel) environmentMap.SetTile(new Vector3Int(x,y,0),treeTile);
            }
        }
        
        groundMap.RefreshAllTiles();
    }

    public void ConvertSeed()
    {
        if (seed.Length <= 0)
        {
            seedFloat = Random.value * Single.MaxValue;
            return;
        }

        foreach (var letter in seed)
        {
            seedFloat += Convert.ToInt32(letter);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }
}
