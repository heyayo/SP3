using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBaseball : MonoBehaviour
{
    public GameObject baseballPrefab; // Assign your baseball prefab in the Inspector
    private Transform firePoint; // Assign the transform where you want to spawn the baseball
    public float upwardForce = 0.4f; // Adjust the force as needed

    // Update is called once per frame
    void Update()
    {
        firePoint = PlayerManager.Instance.transform;
        // Check if the "V" key is pressed
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Call a method to spawn and launch the baseball
            SpawnAndLaunchBaseball();
        }
    }

    void SpawnAndLaunchBaseball()
    {
        // Instantiate the baseball prefab at the firePoint's position and rotation
        GameObject baseballInstance = Instantiate(baseballPrefab, firePoint.position + new Vector3(0,4,0), firePoint.rotation);

        // Get the Rigidbody component of the instantiated baseball
        Rigidbody2D rb = baseballInstance.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Apply an upward impulse force to the baseball
            rb.AddForce(Vector3.up * upwardForce, ForceMode2D.Impulse);

            
            // Change the layer to "Default" initially
            baseballInstance.layer = LayerMask.NameToLayer("Default");

            // Delay changing the layer to "Baseball" after 2 seconds
            StartCoroutine(ChangeLayerAfterDelay(baseballInstance, "baseball", 1f));
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on the baseball prefab!");
        }
    }

    // Coroutine to change the layer after a delay
    private IEnumerator ChangeLayerAfterDelay(GameObject obj, string newLayerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.layer = LayerMask.NameToLayer(newLayerName);
    }
}