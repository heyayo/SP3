using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffGui : MonoBehaviour
{
    [SerializeField] private GameObject guiPanel; // Reference to your GUI panel
    [SerializeField] private GameObject Instrunctionopen;
    [SerializeField] private GameObject Regeneffect;

    private Transform player;    // Reference to the player object
    public float interactionRange = 2.0f; // Range within which the player can interact

    private Mortality playerhp;

    private bool isGuiActive = false;
    private bool wasGamePaused = false;

    private void Start()
    {

       
        player = PlayerManager.Instance.transform;

        playerhp = PlayerManager.Instance.MortalityScript;
        // Hide the GUI panel initially
        guiPanel.SetActive(false);
    }

    private void Update()
    {
        // Calculate the distance between the player and the GUI panel
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= interactionRange)
        {
            Regeneffect.SetActive(true);
            Regeneffect.transform.position = PlayerManager.Instance.transform.position;
            Instrunctionopen.SetActive(true);
            playerhp.Health += 1.0f * Time.deltaTime;

        }
        else
        {
            Regeneffect.SetActive(false);
            Instrunctionopen.SetActive(false);
        }

        // Check for Esc key press only if the player is in range
        if (distanceToPlayer <= interactionRange && Input.GetKeyDown(KeyCode.P))
        {

           

            ToggleGUI();
        }
    }

    private void ToggleGUI()
    {
        // Toggle the active state of the GUI panel
        isGuiActive = !isGuiActive;
        guiPanel.SetActive(isGuiActive);

        // Pause/unpause the game
        if (isGuiActive)
        {
            wasGamePaused = Time.timeScale == 0; // Check if the game was already paused
            if (!wasGamePaused)
            {
                Time.timeScale = 0; // Pause the game
            }
        }
        else
        {
            if (!wasGamePaused)
            {
                Time.timeScale = 1; // Unpause the game
            }
        }
    }
}