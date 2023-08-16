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

    public enum TileID
    {
        GRASS,
        WATER
    }

    public string saveName = "";
    public int width;
    public int height;
    public List<List<TileID>> tiles;

    [ContextMenu("DEBUG_SAVE")]
    public void Save()
    {
        WorldGenOptions config = WorldGenOptions.FetchConfig();
        WorldGenerator generator = WorldGenerator.instance;
        saveName = "TESTSAVE";
        width = config.worldSize.x;
        height = config.worldSize.y;
        tiles = generator.GetMapTileIDs();

        string path = SaveLocation + saveName + ".sav";

        using (var stream = File.Open(path, FileMode.OpenOrCreate))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(saveName); // Write Name
                writer.Write(width); // Write World Width
                writer.Write(height); // Write World Height
                foreach (var x in tiles)
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
        tiles = new List<List<TileID>>();

        using (var stream = File.Open(path, FileMode.Open))
        {
            using (var reader = new BinaryReader(stream))
            {
                saveName = reader.ReadString();
                width = reader.ReadInt32();
                height = reader.ReadInt32();
                for (int i = 0; i < width; ++i)
                {
                    tiles.Add(new List<TileID>());
                    for (int j = 0; j < height; ++j)
                    {
                        tiles[i].Add((TileID)reader.ReadInt32());
                    }
                }
            }
        }
        Debug.Log("Save Read");
    }
}
