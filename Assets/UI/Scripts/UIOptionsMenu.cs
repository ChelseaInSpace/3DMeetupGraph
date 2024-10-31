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

    List<Resolution> filteredResolutions;
    int currentResolutionIndex = -1;
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void OnEnable()
    {
        GenerateResolutionsDropdown();
        FullscreenToggle.isOn = Screen.fullScreen;
        VsyncToggle.isOn = QualitySettings.vSyncCount == 1;
    }

    void GenerateResolutionsDropdown()
    {
        Resolution[] resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        MyDropdown.ClearOptions();
        double currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
                filteredResolutions.Add(resolutions[i]);
        }
        List<string> refreshOptions = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            refreshOptions.Add(option);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        MyDropdown.AddOptions(refreshOptions);
        if(currentResolutionIndex < 0)
            MyDropdown.value = MyDropdown.options.Count - 1;
        else
            MyDropdown.value = currentResolutionIndex;
        MyDropdown.RefreshShownValue();
    }
    
    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ToggleVsync(bool vsyncEnabled)
    {
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
    }
}