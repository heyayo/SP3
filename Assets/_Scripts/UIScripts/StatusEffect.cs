using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffect : MonoBehaviour
{
    public Affliction affliction;
    private Image _statusIcon;

    private void Awake()
    {
        _statusIcon = GetComponent<Image>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _statusIcon.sprite = affliction.icon;
    }
}
