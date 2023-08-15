using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffGui : MonoBehaviour
{
    public GameObject guiPanel; // Reference to your GUI panel
    private bool isGuiActive = false;
    private bool wasGamePaused = false;

    private void Start()
    {
        // Hide the GUI panel initially
        guiPanel.SetActive(false);
    }

    private void Update()
    {
        // Check for Esc key press to toggle GUI and pause/unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
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