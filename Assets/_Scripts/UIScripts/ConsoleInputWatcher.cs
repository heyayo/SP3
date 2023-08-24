using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleInputWatcher : MonoBehaviour
{
    private TMP_InputField input;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DebugConsole.Instance.ParseCommand(input.text);
            input.text = "";
        }
    }
}
