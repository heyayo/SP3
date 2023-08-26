using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skills
{
    public string skillName;
    public List<Skills> prerequisites = new List<Skills>();
    public Button associatedButton;
    public bool unlocked;
}
