using Microsoft.Unity.VisualStudio.Editor;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] WorldGenOptions options;
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private float threshold = 0.5f;
    [SerializeField] private float scale = 0.1f;

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
        Tile grassTile = CreateTile(sprites[0],new Color(0,0.4f,0));
        Tile waterTile = CreateTile(sprites[1], Color.cyan);
        
        for (int i = -options.worldSize.x/2; i < options.worldSize.x/2; ++i)
        {
            for (int j = -options.worldSize.y/2; j < options.worldSize.y/2; ++j)
            {
                float level = Mathf.PerlinNoise(i * scale, j * scale);
                if (level > threshold)
                    groundMap.SetTile(new Vector3Int(i,j,0), grassTile);
                else
                    groundMap.SetTile(new Vector3Int(i,j,0), waterTile);
            }
        }
        
        groundMap.RefreshAllTiles();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }
}
