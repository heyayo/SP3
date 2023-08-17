using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
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
    [Header("Ground Tiles")]
    [SerializeField] private Tile grassTile;
    [SerializeField] private Tile waterTile;

    [Header("Environment Tiles")]
    [SerializeField] private Tile tallGrassTile;
    
    [Header("Interactable Objects")]
    [SerializeField] private ForestTree forestTree;

    [Header("For Debug Visual Only")]
    [SerializeField] private float seedFloat = 0f;

    [Header("Events")]
    public UnityEvent onWorldBeginGen;
    public UnityEvent onWorldEndGen;
    
    // Possibly Internalized Later
    private const float _scale = 0.025f;
    private const float _environmentOffset = Int16.MaxValue;

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
        PlayerManager.Instance.FreezePlayer();
        onWorldBeginGen.Invoke();
        
        ConvertSeed();
        
        int halfX = options.worldSize.x / 2;
        int halfY = options.worldSize.y / 2;

        for (int x = -halfX; x < halfX; ++x)
        {
            for (int y = -halfY; y < halfY; ++y)
            {
                // Determine Ground Tile Level
                float xGround = (x + seedFloat) * _scale;
                float yGround = (y + seedFloat) * _scale;
                float xEnvironment = (x + seedFloat + _environmentOffset) * _scale;
                float yEnvironment = (y + seedFloat + _environmentOffset) * _scale;
                float groundLevel = Mathf.PerlinNoise(xGround, yGround);
                float environmentLevel = Mathf.PerlinNoise(xEnvironment, yEnvironment);
                // Current Tile Location
                Vector3Int tileLoc = new Vector3Int(x, y, 0);
                
                // Place Ground Tiles
                if (groundLevel > options.seaLevel)
                    groundMap.SetTile(tileLoc, grassTile);
                else
                    groundMap.SetTile(tileLoc, waterTile);
                
                // Prevent Environment Tiles on Water
                if (groundLevel <= options.seaLevel) continue;

                if (environmentLevel >= options.grassLevel)
                    environmentMap.SetTile(tileLoc, tallGrassTile);
                    
                // Place Tree Tiles
                if (groundLevel > options.treeLevel)
                {
                    Instantiate(forestTree, tileLoc, Quaternion.identity);
                }
            }
        }
        
        groundMap.RefreshAllTiles();
        
        onWorldEndGen.Invoke();
        PlayerManager.Instance.UnFreezePlayer();
    }

    // Might Replace Above
    public async Task GenerateWorldAsync()
    {
        PlayerManager.Instance.FreezePlayer();
        onWorldBeginGen.Invoke();
        
        ConvertSeed();
        
        int halfX = options.worldSize.x / 2;
        int halfY = options.worldSize.y / 2;

        for (int x = -halfX; x < halfX; ++x)
        {
            for (int y = -halfY; y < halfY; ++y)
            {
                // Determine Ground Tile Level
                float xGround = (x + seedFloat) * _scale;
                float yGround = (y + seedFloat) * _scale;
                float xEnvironment = (x + seedFloat + _environmentOffset) * _scale;
                float yEnvironment = (y + seedFloat + _environmentOffset) * _scale;
                float groundLevel = Mathf.PerlinNoise(xGround, yGround);
                float environmentLevel = Mathf.PerlinNoise(xEnvironment, yEnvironment);
                // Current Tile Location
                Vector3Int tileLoc = new Vector3Int(x, y, 0);
                
                // Place Ground Tiles
                if (groundLevel > options.seaLevel)
                    groundMap.SetTile(tileLoc, grassTile);
                else
                    groundMap.SetTile(tileLoc, waterTile);
                
                // Prevent Environment Tiles on Water
                if (groundLevel <= options.seaLevel) continue;

                if (environmentLevel >= options.grassLevel)
                    environmentMap.SetTile(tileLoc, tallGrassTile);
                    
                // Place Tree Tiles
                if (groundLevel > options.treeLevel)
                {
                    Instantiate(forestTree, tileLoc, Quaternion.identity);
                }

                await Task.Yield();
            }
        }
        
        groundMap.RefreshAllTiles();
        
        onWorldEndGen.Invoke();
        PlayerManager.Instance.UnFreezePlayer();
    }
    
    // Depreciated
    [ContextMenu("DEBUG_LOADFROMSAVE")]
    public void LoadWorldFromSave()
    {
        PlayerManager.Instance.FreezePlayer();
        SaveGame.Instance.Load();
        
        for (int i = 0; i < options.worldSize.x; ++i)
        {
            for (int j = 0; j < options.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - options.worldSize.x/2, j - options.worldSize.y/2, 0);
                SaveGame.GroundTileID groundTileID = SaveGame.Instance.groundTiles[i][j];
                switch (groundTileID)
                {
                    case SaveGame.GroundTileID.GRASS:
                    {
                        groundMap.SetTile(tileLoc,grassTile);
                        break;
                    }
                    case SaveGame.GroundTileID.WATER:
                    {
                        groundMap.SetTile(tileLoc,waterTile);
                        break;
                    }
                }
            }
        }
        PlayerManager.Instance.UnFreezePlayer(); 
    }

    public void ConvertSeed()
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

    public List<List<SaveGame.GroundTileID>> GetGroundMapTileIDs()
    {
        List<List<SaveGame.GroundTileID>> result = new List<List<SaveGame.GroundTileID>>();
        for (int i = 0; i < options.worldSize.x; ++i)
        {
            result.Add(new List<SaveGame.GroundTileID>());
            for (int j = 0; j < options.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - options.worldSize.x/2, j - options.worldSize.y/2, 0);
                var tile = groundMap.GetTile(tileLoc);

                if (tile == grassTile)
                    result[i].Add(SaveGame.GroundTileID.GRASS);
                else if (tile == waterTile)
                    result[i].Add(SaveGame.GroundTileID.WATER);
            }
        }

        return result;
    }
    
    public List<List<SaveGame.EnvironmentTileID>> GetEnvironmentMapTileIDs()
    {
        List<List<SaveGame.EnvironmentTileID>> result = new List<List<SaveGame.EnvironmentTileID>>();
        for (int i = 0; i < options.worldSize.x; ++i)
        {
            result.Add(new List<SaveGame.EnvironmentTileID>());
            for (int j = 0; j < options.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - options.worldSize.x/2, j - options.worldSize.y/2, 0);
                var tile = environmentMap.GetTile(tileLoc);

                if (tile == tallGrassTile)
                    result[i].Add(SaveGame.EnvironmentTileID.TallGrass);
            }
        }

        return result;
    }
}
