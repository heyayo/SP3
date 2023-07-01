using UnityEngine;

[CreateAssetMenu(fileName="World Generation Options",menuName="WorldGen/Options",order=1)]
public class WorldGenOptions : ScriptableObject
{
    public Vector2Int worldSize = Vector2Int.zero;
    public float seaLevel = .25f;
    public float treeLevel = 0.8f;
}
