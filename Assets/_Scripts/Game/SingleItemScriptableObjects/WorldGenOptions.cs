using UnityEngine;

[CreateAssetMenu(fileName="World Generation Options",menuName="Options/WorldGen",order=1)]
public class WorldGenOptions : ScriptableObject
{
    public static WorldGenOptions FetchConfig()
    {
        return Resources.Load<WorldGenOptions>("configs/worldgen_cfg");
    }
    
    public Vector2Int worldSize = Vector2Int.zero;
    public float seaLevel = .25f;
    public float treeLevel = 0.8f;
    public float grassLevel = 0.5f;
    public float natureLevel = 0.4f;
    public string seedString;
}
