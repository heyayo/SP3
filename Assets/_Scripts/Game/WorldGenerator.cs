using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator instance { get; private set; }
    
    [Header("Options Scriptable Object")]
    [SerializeField] WorldGenOptions options;
    
    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap environmentMap;

    [Header("Hardcoded Tile Resources")]
    [SerializeField] private Tile grassTile;
    [SerializeField] private Tile waterTile;
    [SerializeField] private Sprite[] environmentTiles;
    [SerializeField] private ForestTree forestTree;

    [Header("For Debug Visual Only")]
    [SerializeField] private float seedFloat = 0f;

    [Header("Events")]
    public UnityEvent onWorldBeginGen;
    public UnityEvent onWorldEndGen;
    
    // Possibly Internalized Later
    private float scale = 0.025f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple World Generators In Scene");
            Debug.Break();
        }
        instance = this;
        onWorldBeginGen = new UnityEvent();
        onWorldEndGen = new UnityEvent();
        // Fetch Components
        groundMap = transform.GetChild(0).GetComponent<Tilemap>();
        environmentMap = transform.GetChild(1).GetComponent<Tilemap>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (environmentTiles.Length == 0)
        {
            Debug.LogError("No Environment Tiles");
            Debug.Break();
        }

        GenerateWorld();
    }
    
    private Tile CreateTile(Sprite sprite, Color color)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = color;
        Debug.Log("Loaded Tile | " + tile.name);
        return tile;
    }

    [ContextMenu("DEBUG_GENERATEWORLD")]
    private void GenerateWorld()
    {
        onWorldBeginGen.Invoke();
        
        ConvertSeed();
        
        // Environment Tiles
        Tile treeTile = CreateTile(environmentTiles[0], Color.green);
        
        for (int x = -options.worldSize.x/2; x < options.worldSize.x/2; ++x)
        {
            for (int y = -options.worldSize.y/2; y < options.worldSize.y/2; ++y)
            {
                // Determine Ground Tile Level
                float xJustified = (x + seedFloat) * scale;
                float yJustified = (y + seedFloat) * scale;
                float groundLevel = Mathf.PerlinNoise(xJustified, yJustified);
                // Current Tile Location
                Vector3Int tileLoc = new Vector3Int(x, y, 0);
                
                // Place Ground Tiles
                if (groundLevel > options.seaLevel)
                    groundMap.SetTile(tileLoc, grassTile);
                else
                    groundMap.SetTile(tileLoc, waterTile);
                
                // Prevent Environment Tiles on Water
                if (groundLevel <= options.seaLevel) continue;
                
                // Place Environment Tiles
                if (groundLevel > options.treeLevel)
                {
                    Instantiate(forestTree, tileLoc, Quaternion.identity);
                }
            }
        }
        
        groundMap.RefreshAllTiles();
        
        onWorldEndGen.Invoke();
    }

    private void ConvertSeed()
    {
        if (options.seedString.Length <= 0)
        {
            seedFloat = Random.value * Single.MaxValue;
            return;
        }

        foreach (var letter in options.seedString)
        {
            seedFloat += Convert.ToInt32(letter);
        }
    }

    public List<List<SaveGame.TileID>> GetMapTileIDs()
    {
        List<List<SaveGame.TileID>> result = new List<List<SaveGame.TileID>>();
        for (int i = 0; i < options.worldSize.x; ++i)
        {
            result.Add(new List<SaveGame.TileID>());
            for (int j = 0; j < options.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - options.worldSize.x/2, j - options.worldSize.y/2, 0);
                var tile = groundMap.GetTile(tileLoc);

                if (tile == grassTile)
                    result[i].Add(SaveGame.TileID.GRASS);
                else if (tile == waterTile)
                    result[i].Add(SaveGame.TileID.WATER);
            }
        }

        return result;
    }

    [ContextMenu("DEBUG_LOADFROMSAVE")]
    public void LoadWorldFromSave()
    {
        SaveGame.Instance.Load();
        
        for (int i = 0; i < options.worldSize.x; ++i)
        {
            for (int j = 0; j < options.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - options.worldSize.x/2, j - options.worldSize.y/2, 0);
                SaveGame.TileID tileID = SaveGame.Instance.tiles[i][j];
                switch (tileID)
                {
                    case SaveGame.TileID.GRASS:
                    {
                        groundMap.SetTile(tileLoc,grassTile);
                        break;
                    }
                    case SaveGame.TileID.WATER:
                    {
                        groundMap.SetTile(tileLoc,waterTile);
                        break;
                    }
                }
            }
        }
    }
}
