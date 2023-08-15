using UnityEngine;

public class ForestTree : Interactable
{
    [SerializeField] private int chopProgress;
    [SerializeField] private int treeStrength;

    public override void OnInteract()
    {
        ++chopProgress;
        if (chopProgress >= treeStrength)
            Destroy(gameObject);
    }
}
