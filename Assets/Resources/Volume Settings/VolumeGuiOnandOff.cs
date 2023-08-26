using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeGuiOnandOff : MonoBehaviour
{
    [SerializeField] GameObject Volumeui;
    private bool active;
    private void Update()
    {
        Volumeui.SetActive(active);
    }
    public void ClickVolumeSettings()
    {
        active = !active;

    }

    public void DisableVolumeSettings()
    {
        active = false;
    }
}
