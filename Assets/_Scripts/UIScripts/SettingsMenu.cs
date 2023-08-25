using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    Resolution[] res;
    public TMPro.TMP_Dropdown resDD;
    private void Start()
    {
        res = Screen.resolutions;

        resDD.ClearOptions();
        List<string> options = new List<string>();
        int curresIndex = 0;
        for (int i = 0; i < res.Length; ++i)
        {
            string option = res[i].width + "x" + res[i].height;
            options.Add(option);

            if (res[i].width == Screen.currentResolution.width && res[i].height == Screen.currentResolution.height)
            {
                curresIndex = i;
            }
        }
        resDD.AddOptions(options);
        resDD.value = curresIndex;
        resDD.RefreshShownValue();
    }
    public void SetRes(int resIndex)
    {
        Resolution resolution = res[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float vol)
    {
        Debug.Log(masterSlider.value);
        mixer.SetFloat("MasterVolume", masterSlider.value);
    }
    public void SetScene()
    {
        SceneManager.LoadScene(1);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
