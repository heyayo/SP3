using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
	[SerializeField] private Toggle fullscreenButton;
	[SerializeField] private TMP_Dropdown resDropdown;
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider bgmSlider;
	[SerializeField] private AudioMixer mixer;
	
	private void Start()
	{
		Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
		Screen.fullScreen = false;
		fullscreenButton.isOn = Screen.fullScreen;

		foreach (var res in Screen.resolutions)
		{
			StringBuilder r = new StringBuilder();
			r.Append(res.width.ToString());
			r.Append('x');
			r.Append(res.height.ToString());
			resDropdown.options.Add(new TMP_Dropdown.OptionData(r.ToString()));
		}

		var setres = Screen.resolutions[Screen.resolutions.Length - 1];
		Screen.SetResolution(setres.width,setres.height,FullScreenMode.FullScreenWindow);
		resDropdown.value = Screen.resolutions.Length - 1;

		float vol;
		if (mixer.GetFloat("SFMVol", out vol))
			sfxSlider.value = vol;
		if (mixer.GetFloat("BGMVol", out vol))
			bgmSlider.value = vol;
	}
	
	public void Fullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}
	
	public void Apply()
	{
		SetResolution();
		SetVolumes();
	}

	private void SetResolution()
	{
		int res = resDropdown.value;
		var reses = Screen.resolutions;
		Screen.SetResolution(reses[res].width,reses[res].height,Screen.fullScreenMode);
	}

	private void SetVolumes()
	{
		mixer.SetFloat("BGMVol", bgmSlider.value);
		mixer.SetFloat("SFXVol", sfxSlider.value);
	}
}
