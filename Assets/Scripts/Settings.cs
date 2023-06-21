using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Dropdown resolutionDropDown;
    public Dropdown qualityDropDown;

    Resolution[] resolutions;

    void Start()
    {
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropDown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropDown.value);
        PlayerPrefs.SetInt("FullScreenPreference", System.Convert.ToInt32(Screen.fullScreen));
    }

    public void LoadSettings(int resolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropDown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropDown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropDown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropDown.value = resolutionIndex;

        if (PlayerPrefs.HasKey("FullScreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreenPreference"));
        
        else
            Screen.fullScreen = true;
    }
}
