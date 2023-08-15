using UnityEngine;

public class KamehamehaAttack : MonoBehaviour
{
    public GameObject kamehamehaPrefab; // Assign the Kamehameha beam prefab in the Inspector
    public Transform firePoint; // The point from where the Kamehameha is fired
    public float beamWidth = 0.1f;
    public float beamMaxLength = 2000f;

    private LineRenderer activeLineRenderer; // To store the active LineRenderer while firing
    private bool isFiring = false; // To track if the Kamehameha is currently firing

    private void Update()
    {
        // Check for input to start or stop firing the Kamehameha
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartFiring();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopFiring();
        }

        // If firing, update the beam's endpoint based on the mouse position
        if (isFiring)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - firePoint.position).normalized;

            // Ensure the beam is always at its maximum length
            Vector3 beamEndpoint = firePoint.position + direction * beamMaxLength;

            activeLineRenderer.SetPosition(1, beamEndpoint);
        }
    }

    private void StartFiring()
    {
        if (!isFiring)
        {
            isFiring = true;
            GameObject kamehameha = Instantiate(kamehamehaPrefab, firePoint.position, Quaternion.identity);
            activeLineRenderer = kamehameha.GetComponent<LineRenderer>();

            // Set the beam width
            activeLineRenderer.startWidth = beamWidth;
            activeLineRenderer.endWidth = beamWidth;

            // Set the LineRenderer starting position
            activeLineRenderer.SetPosition(0, firePoint.position);
        }
    }

    private void StopFiring()
    {
        if (isFiring)
        {
            isFiring = false;
            Destroy(activeLineRenderer.gameObject);
        }
    }
}