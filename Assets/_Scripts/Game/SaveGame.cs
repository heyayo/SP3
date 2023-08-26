using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class SaveGame : MonoBehaviour
{
    public static SaveGame Instance { get; private set; }
    public static string SaveLocation;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Save Managers In Scene");
            Debug.Break();
        }
        Instance = this;
        SaveLocation = Path.Combine(Application.dataPath, "Saves/");
        if (!Directory.Exists(SaveLocation))
        {
            Directory.CreateDirectory(SaveLocation);
            Debug.LogWarning("Saves Directory Not Found | Creating");
        }
    }

    public enum GroundTileID
    {
        GRASS,
        WATER
    }

    public enum EnvironmentTileID
    {
        TallGrass
    }

    public enum Interactable
    {
        ForestTree,
        DragonBall
    }

    public string saveName = "";
    public int width;
    public int height;
    public List<List<GroundTileID>> groundTiles;
    public List<List<EnvironmentTileID>> environmentTiles;
    public List<List<Interactable>> interactables;

    [ContextMenu("DEBUG_SAVE")]
    public void Save()
    {
        WorldGenerator generator = WorldGenerator.instance;
        saveName = "TESTSAVE";
        width = WorldGenOptions.worldSize.x;
        height = WorldGenOptions.worldSize.y;
        groundTiles = generator.GetGroundMapTileIDs();
        environmentTiles = generator.GetEnvironmentMapTileIDs();

        string path = SaveLocation + saveName + ".sav";

        using (var stream = File.Open(path, FileMode.OpenOrCreate))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(saveName); // Write Name
                writer.Write(width); // Write World Width
                writer.Write(height); // Write World Height
                // Write Ground Tiles
                foreach (var x in groundTiles)
                {
                    foreach (var y in x)
                    {
                        writer.Write((int)y); // Write Tile
                    }
                }
                // Write Environment Tiles
                foreach (var x in groundTiles)
                {
                    foreach (var y in x)
                    {
                        writer.Write((int)y); // Write Tile
                    }
                }
            }
        }
        Debug.Log("Save Write");
    }

    public void Load()
    {
        string path = SaveLocation + saveName + ".sav";
        PrepareList(ref groundTiles, width);
        PrepareList(ref environmentTiles, width);
        

        using (var stream = File.Open(path, FileMode.Open))
        {
            using (var reader = new BinaryReader(stream))
            {
                saveName = reader.ReadString();
                width = reader.ReadInt32();
                height = reader.ReadInt32();
                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        groundTiles[i].Add((GroundTileID)reader.ReadInt32());
                    }
                }
            }
        }
        Debug.Log("Save Read");
    }

    private void PrepareList<T>(ref List<List<T>> list, int size)
    {
        list = new List<List<T>>();
        for (int i = 0; i < size; ++i)
        {
            list.Add(new List<T>());
        }
    }
}
