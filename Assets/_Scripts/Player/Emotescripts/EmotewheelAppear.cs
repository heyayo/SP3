using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotewheelAppear : MonoBehaviour
{
    private bool guiVisible = false;
    [SerializeField] private GameObject Emotewheel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            guiVisible = !guiVisible; // Toggle the GUI visibility
        }

        Emotewheel.SetActive(guiVisible);
    }




}