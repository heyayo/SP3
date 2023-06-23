using Microsoft.Unity.VisualStudio.Editor;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] WorldGenOptions options;
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Sprite[] sprites;

    private Tile CreateTile(Sprite sprite, Color color)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = color;
        Debug.Log("Loaded Tile | " + sprite.name);
        return tile;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Tile grassTile = CreateTile(sprites[0],new Color(0,0.4f,0));

        for (int i = -options.worldSize.x/2; i < options.worldSize.x/2; ++i)
        {
            for (int j = -options.worldSize.y/2; j < options.worldSize.y/2; ++j)
            {
                groundMap.SetTile(new Vector3Int(i,j,0), grassTile);
            }
        }
        
        groundMap.RefreshAllTiles();
    }
}
