using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeStopPlayerScript : MonoBehaviour
{
    private bool isPaused = false;
    private bool canToggle = true;
    private float toggleCooldown = 2f; // Cooldown time in seconds


    [SerializeField] public GameObject Timestop;

    private PlayerProjectileScript PlayerKi;

    void Update()
    {
        ToggleEnemyMovement();
        // Check for the "T" key press
        if (Input.GetKeyDown(KeyCode.T) && canToggle)
        {
            StartCoroutine(ToggleCooldown()); // Start cooldown coroutine
            PlayerKi = GameObject.FindWithTag("Player").GetComponent<PlayerProjectileScript>();
            Timestop.SetActive(true);
            PlayerKi.timeisstopped = true;
            
            isPaused = true; // Toggle the isPaused flag
        }
    }

    IEnumerator ToggleCooldown()
    {

        canToggle = false; // Disable toggling
        yield return new WaitForSeconds(toggleCooldown);
        canToggle = true; // Enable toggling after cooldown

        // Deactivate the GUI here
        Timestop.SetActive(false);

        // Wait for additional time before executing the L key actions
      
       

        // Run the L key actions here
        PlayerKi = GameObject.FindWithTag("Player").GetComponent<PlayerProjectileScript>();
        PlayerKi.UntimeStop();
        PlayerKi.timeisstopped = false;
        
        isPaused = false; // Toggle the isPaused flag

    }


    void ToggleEnemyMovement()
    {
        TargetDummyEnemy[] enemies = FindObjectsOfType<TargetDummyEnemy>();
      

        foreach (TargetDummyEnemy enemy in enemies)
        {
            if (isPaused)
            {

                enemy.PauseMovement();
              
            }
            else
            {
                enemy.ResumeMovement();
              
            }
        }

        //foreach (KiProjectile Kis in Ki)
        //{
        //    if (isPaused)
        //    {

        //        Kis.PauseMovement();
        //        Timestop.SetActive(true);
        //    }
        //    else
        //    {
        //         Kis.ResumeMovement();
        //        Timestop.SetActive(false);
        //    }
        //}
    }
}