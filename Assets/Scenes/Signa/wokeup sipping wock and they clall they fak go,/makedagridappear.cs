using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makedagridappear : MonoBehaviour
{
    public GameObject prefabToInstantiate; // Assign your prefab in the Inspector
    public float minInterval = 1f; // Minimum time interval in seconds
    public float maxInterval = 2f; // Maximum time interval in seconds

    private float timeSinceLastInstantiation;
    private float nextInstantiationTime;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the time counters
        timeSinceLastInstantiation = 0f;
        nextInstantiationTime = Random.Range(minInterval, maxInterval);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the time counters
        timeSinceLastInstantiation += Time.deltaTime;

        // Check if it's time to instantiate the prefab
        if (timeSinceLastInstantiation >= nextInstantiationTime)
        {
            // Instantiate the prefab at the current position
            Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);

            // Reset the time counters
            timeSinceLastInstantiation = 0f;
            nextInstantiationTime = Random.Range(minInterval, maxInterval);
        }
    }
}