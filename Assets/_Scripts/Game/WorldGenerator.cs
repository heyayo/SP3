using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator instance { get; private set; }
    
    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap environmentMap;

    [Header("Hardcoded Tile Resources")]
    [Header("Ground Tiles")]
    [SerializeField] private Tile grassTile;
    [SerializeField] private AnimatedTile waterTile;

    [Header("Environment Tiles")]
    [SerializeField] private Tile tallGrassTile;
    
    [Header("Interactable Objects")]
    [SerializeField] private ForestTree forestTree;
    [SerializeField] private IronOre ironOre;

    [Header("For Debug Visual Only")]
    [SerializeField] private float seedFloat = 0f;

    [Header("Events")]
    public UnityEvent onWorldBeginGen;
    public UnityEvent onWorldEndGen;
    
    // Possibly Internalized Later
    private const float _scale = 0.025f;
    private const float _environmentOffset = Int16.MaxValue;

    private EdgeCollider2D _border;

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
        _border = GetComponent<EdgeCollider2D>();
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

        Vector2 offset = new Vector2(0.5f, 0.5f);
        
        int halfX = WorldGenOptions.worldSize.x / 2;
        int halfY = WorldGenOptions.worldSize.y / 2;

        for (int x = -halfX; x < halfX; ++x)
        {
            for (int y = -halfY; y < halfY; ++y)
            {
                // Determine Ground Tile Level
                float xGround = (x + seedFloat) * _scale;
                float yGround = (y + seedFloat) * _scale;
                float xEnvironment = (x + seedFloat + _environmentOffset) * 0.4f;
                float yEnvironment = (y + seedFloat + _environmentOffset) * 0.4f;
                float groundLevel = Mathf.PerlinNoise(xGround, yGround);
                float natureLevel = Mathf.PerlinNoise((x + seedFloat) * 0.2f, (y + seedFloat) * 0.2f);
                float biomeLevel = Mathf.PerlinNoise((x + seedFloat) * 0.01f, (y + seedFloat) * 0.01f);
                float environmentLevel = Mathf.PerlinNoise(xEnvironment, yEnvironment);
                // Current Tile Location
                Vector3Int tileLoc = new Vector3Int(x, y, 0);
                
                // Place Ground Tiles
                if (groundLevel > WorldGenOptions.seaLevel)
                    groundMap.SetTile(tileLoc, grassTile);
                else
                    groundMap.SetTile(tileLoc, waterTile);
                
                // Prevent Environment Tiles on Water
                if (groundLevel <= WorldGenOptions.seaLevel) continue;

                if (environmentLevel >= WorldGenOptions.grassLevel && natureLevel > WorldGenOptions.natureLevel && biomeLevel > 0.4f && biomeLevel < 0.6f)
                {
                    environmentMap.SetTile(tileLoc, tallGrassTile);
                    continue;
                }

                Vector2 offsetedTileLoc = (Vector2)(Vector3)tileLoc + offset;
                // Place Tree Tiles
                if (biomeLevel < 0.2f)
                {
                    if (natureLevel < WorldGenOptions.natureLevel)
                        Instantiate(ironOre, offsetedTileLoc, Quaternion.identity);
                }
                else if (biomeLevel > 0.65f)
                {
                    if (groundLevel > WorldGenOptions.treeLevel && natureLevel > WorldGenOptions.natureLevel)
                    {
                        Instantiate(forestTree, offsetedTileLoc, Quaternion.identity);
                    }
                }
            }
        }
        
        groundMap.RefreshAllTiles();
        
        onWorldEndGen.Invoke();
        PlayerManager.Instance.UnFreezePlayer();
        
        MatchBorder();
    }

    private void MatchBorder()
    {
        List<Vector2> points = new List<Vector2>();
        points.Add(new Vector2(-WorldGenOptions.worldSize.x, WorldGenOptions.worldSize.y)/2);
        points.Add(new Vector2(WorldGenOptions.worldSize.x, WorldGenOptions.worldSize.y)/2);
        points.Add(new Vector2(WorldGenOptions.worldSize.x, -WorldGenOptions.worldSize.y)/2);
        points.Add(new Vector2(-WorldGenOptions.worldSize.x, -WorldGenOptions.worldSize.y)/2);
        points.Add(new Vector2(-WorldGenOptions.worldSize.x, WorldGenOptions.worldSize.y)/2);
        _border.SetPoints(points);
    }

    // Depreciated
    [ContextMenu("DEBUG_LOADFROMSAVE")]
    public void LoadWorldFromSave()
    {
        PlayerManager.Instance.FreezePlayer();
        SaveGame.Instance.Load();
        
        for (int i = 0; i < WorldGenOptions.worldSize.x; ++i)
        {
            for (int j = 0; j < WorldGenOptions.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - WorldGenOptions.worldSize.x/2, j - WorldGenOptions.worldSize.y/2, 0);
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
        if (WorldGenOptions.seedString.Length <= 0)
        {
            seedFloat = Random.value * Single.MaxValue;
            return;
        }

        foreach (var letter in WorldGenOptions.seedString)
        {
            seedFloat += Convert.ToInt32(letter);
        }
    }

    public List<List<SaveGame.GroundTileID>> GetGroundMapTileIDs()
    {
        List<List<SaveGame.GroundTileID>> result = new List<List<SaveGame.GroundTileID>>();
        for (int i = 0; i < WorldGenOptions.worldSize.x; ++i)
        {
            result.Add(new List<SaveGame.GroundTileID>());
            for (int j = 0; j < WorldGenOptions.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - WorldGenOptions.worldSize.x/2, j - WorldGenOptions.worldSize.y/2, 0);
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
        for (int i = 0; i < WorldGenOptions.worldSize.x; ++i)
        {
            result.Add(new List<SaveGame.EnvironmentTileID>());
            for (int j = 0; j < WorldGenOptions.worldSize.y; ++j)
            {
                Vector3Int tileLoc = new Vector3Int(i - WorldGenOptions.worldSize.x/2, j - WorldGenOptions.worldSize.y/2, 0);
                var tile = environmentMap.GetTile(tileLoc);

                if (tile == tallGrassTile)
                    result[i].Add(SaveGame.EnvironmentTileID.TallGrass);
            }
        }

        return result;
    }
}
