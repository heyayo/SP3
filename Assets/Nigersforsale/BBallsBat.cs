using UnityEngine;

[CreateAssetMenu(menuName = "Items/Baseball")]
public class BBallsBat : Item
{
    [SerializeField] GameObject BBALLBATPrefab; // Prefab reference
    GameObject instantiatedBat; // Reference to the instantiated bat
    bool active;

    public override void Use()
    {
        active = !active;

        // Instantiate the bat prefab when it's used for the first time
        if (instantiatedBat == null && active && BBALLBATPrefab != null)
        {
            instantiatedBat = Instantiate(BBALLBATPrefab);
            //instantiatedBat.SetActive(true);
        }

        // Toggle the GameObject's active state
        if (instantiatedBat != null)
            instantiatedBat.SetActive(active);
    }

    public override void WhileHolding()
    {
        // This function can be empty if the Use function handles activation and deactivation.
        // You can add additional behavior here if needed.
    }
}