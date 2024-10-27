using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOptionsMenu : MonoBehaviour
{
    public TMP_Dropdown MyDropdown;
    public Toggle FullscreenToggle;
    public Toggle VsyncToggle;
    
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    double currentRefreshRate;
    int currentResolutionIndex = 0;
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, SettingsData.Fullscreen);
        SettingsData.ResolutionWidth = resolution.width;
        SettingsData.ResolutionHeight = resolution.height;
        SettingsData.RefreshRate = resolution.refreshRateRatio.value;
    }

    private void OnEnable()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        MyDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].refreshRateRatio.value == currentRefreshRate)
                filteredResolutions.Add(resolutions[i]);
        }
        List<string> RefreshOptions = new List<string>();
        for(int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            RefreshOptions.Add(option);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        MyDropdown.AddOptions(RefreshOptions);
        MyDropdown.value = currentResolutionIndex;
        MyDropdown.RefreshShownValue();
        FullscreenToggle.isOn = SettingsData.Fullscreen;
        VsyncToggle.isOn = SettingsData.VSync;
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        SettingsData.Fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
        MyDropdown.value = currentResolutionIndex;
    }

    public void ToggleVsync(bool vsyncEnabled)
    {
        SettingsData.VSync = vsyncEnabled;
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
    }
}