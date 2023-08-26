using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotewheel : MonoBehaviour
{
    private bool guiVisible = false;
    [SerializeField] private GameObject EmotewheelGui;

    private bool keeganking;


    private void Start()
    {
        keeganking = false;
    }
    void Update()
    {
        Animator lmao = PlayerManager.Instance.GetComponent<Animator>();

        if (Input.GetKeyDown(KeyCode.B))
        {
          
            guiVisible = !guiVisible; // Toggle the GUI visibility
        }

        if(keeganking == true)
        {
            lmao.Play("Griddy");
        }

        EmotewheelGui.SetActive(guiVisible);
    }

    public void Griddy()
    {
       
        keeganking = !keeganking;
        guiVisible = false; 

    }
}
