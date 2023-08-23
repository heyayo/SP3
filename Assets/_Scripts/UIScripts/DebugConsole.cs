using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Command
{
    public abstract void Action(string[] args);
    public abstract string HelpMessage();
}

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole Instance { get; private set; }

    public Dictionary<string, Command> commands;

    public TMP_Text outputText;

    private Configuration _config;
    private GameObject _console;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Debug Consoles in Scene");
            Debug.Break();
        }
        Instance = this;
        _config = Configuration.FetchConfig();

        commands = new Dictionary<string, Command>();

        commands.Add("help",new Help());
        commands.Add("additem",new AddItem());
    }

    private void Start()
    {
        outputText.text = "";
        _console = transform.GetChild(0).gameObject;
        _console.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_config.consoleOpen))
        {
            _console.SetActive(!_console.activeInHierarchy);
        }
    }
    
    public void ParseCommand(string command)
    {
        command = command.ToLower();
        string[] pieces = command.Split(' ');
        commands[pieces[0]].Action(pieces);
    }
}

public class Help : Command
{
    public override void Action(string[] args)
    {
        foreach (var command in DebugConsole.Instance.commands)
        {
            DebugConsole.Instance.outputText.text += command.Key + " | " + command.Value.HelpMessage() + '\n';
        }
    }

    public override string HelpMessage()
    {
        return "Prints out possible commands";
    }
}

public class AddItem : Command
{
    public override void Action(string[] args)
    {
        var item = Resources.Load<Item>(args[1]);
        if (item == null)
        {
            DebugConsole.Instance.outputText.text += args[1] + " Does not Exist";
            return;
        }
        InventoryManager.Instance.Add(item);
    }   

    public override string HelpMessage()
    {
        return "Adds an Item to Inventory";
    }
}
