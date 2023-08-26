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

    //[SerializeField] GameObject BBALLBAT;
    //bool active;
    //// Method to toggle the GameObject's active state
    //public void ToggleGameObject(bool isActive)
    //{
    //    BBALLBAT.SetActive(isActive);
    //}


    //private void OnEnable()
    //{
    //    BBALLBAT = Instantiate(BBALLBAT, Vector3.zero, Quaternion.identity);
    //    BBALLBAT.SetActive(false);
    //}

    //private void OnDisable()
    //{
    //    Destroy(BBALLBAT);
    //}

    //public override void Use()
    //{
    //    active = !active;
        

    //}

    //public override void WhileHolding()
    //{
    //    ToggleGameObject(active);
    //}
