using UnityEngine;

public class DragonBall : Interactable
{
    public override void OnInteract()
    {
        Debug.Log("Picked Up Dragon Ball");
        Destroy(gameObject);
    }
}
