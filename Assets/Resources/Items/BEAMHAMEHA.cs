using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/BEAMATTACK")]
public class BEAMHAMEHA : Item
{

    //public GameObject kamehamehaParticleEffect;
    public GameObject beamPrefab;
    private Transform firePoint; // Assign the fire point transform in the Inspector
    private GameObject currentBeam;
    private bool charging = false;
    private bool beamActive = false;
    private float chargeStartTime;
    private float beamExistenceTime = 5f; // Time the beam can exist after release

    public float maxScale = 20f; // Maximum scale of the beam
    public float chargeRate = 70.5f; // Rate at which the beam grows
    private CameraController cameraController;

    private void OnEnable()
    {

    }
    

    public override void WhileHolding()
    {
        firePoint = PlayerManager.Instance.gameObject.transform;

        cameraController = FindObjectOfType<CameraController>();
        if (cameraController == null)
        {
            Debug.LogError("CameraController not found in the scene.");
        }

        if (Input.GetKeyDown(KeyCode.R) && !charging && !beamActive)
        {
            charging = true;
            chargeStartTime = Time.time;
            //kamehamehaParticleEffect.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.R) && charging && !beamActive)
        {
            charging = false;
            beamActive = true;
            currentBeam = Instantiate(beamPrefab, firePoint.position, Quaternion.identity);
            // Play the charging particle effect
            //kamehamehaParticleEffect.SetActive(false);
        }
        if (beamActive && currentBeam != null)
        {
            // Calculate the direction from firePoint to mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - firePoint.position;

            // Calculate the angle based on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation of the beam
            Quaternion beamRotation = Quaternion.Euler(0f, 0f, angle);

            // Update the beam's rotation and position to match firePoint's position and the calculated rotation
            currentBeam.transform.position = firePoint.position;
            currentBeam.transform.rotation = beamRotation;

            // Calculate the scale of the beam based on charging duration
            float chargeDuration = Time.time - chargeStartTime;

            float beamScale = Mathf.Clamp(chargeDuration * chargeRate, 0f, maxScale);
            // Calculate the scale of the beam along the y-axis based on charging duration
            float beamScaleY = Mathf.Clamp(chargeDuration * chargeRate, 0f, 2); // You'll need to define maxScaleY

            // Update the scale of the beam GameObject
            currentBeam.transform.localScale = new Vector3(beamScale, beamScale, 1f);

            // Add random rotation for the shaking effect
            float shakeAmount = 1.0f; // Adjust the amount of shake as needed
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(-shakeAmount, shakeAmount));
            currentBeam.transform.rotation *= randomRotation;

            if (cameraController != null)
            {
                cameraController.StartCameraShake();
            }
        }

        // Countdown and delete beam when its existence time runs out
        if (beamActive && currentBeam != null)
        {
            beamExistenceTime -= Time.deltaTime;
            if (beamExistenceTime <= 0f)
            {
                beamActive = false;
                Destroy(currentBeam);
                beamExistenceTime = 5f; // Reset beam existence time
            }
        }
    }

  
}
