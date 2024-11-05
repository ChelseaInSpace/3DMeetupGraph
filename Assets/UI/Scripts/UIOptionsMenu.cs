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
    public Toggle ControlsInvertX;
    public Toggle ControlsInvertY;
    public Toggle ControlsInvertZoom;
    public Toggle ControlsInvertDrag;
    public Toggle ControlsMouseLock;
    public Toggle ControlsAutocolourNodes;
    public Toggle ControlsCameraMovement;

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
        GenerateControlSettings();
        FullscreenToggle.isOn = Screen.fullScreen;
        VsyncToggle.isOn = QualitySettings.vSyncCount == 1;
    }

    void OnDisable()
    {
        SettingsManager.SaveSettings();
    }

    void GenerateControlSettings()
    {
        ControlsInvertX.isOn = SettingsData.InvertCamRotX;
        ControlsInvertY.isOn = SettingsData.InvertCamRotY;
        ControlsInvertZoom.isOn = SettingsData.InvertScroll;
        ControlsInvertDrag.isOn = SettingsData.InvertDrag;
        ControlsMouseLock.isOn = SettingsData.LockMouseOnCameraMovement;
        ControlsAutocolourNodes.isOn = SettingsData.RecolourNodesOnPositioning;
        ControlsCameraMovement.isOn = SettingsData.MoveCamOnSelection;
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

    public void ToggleInvertX(bool isInvertX)
    {
        SettingsData.InvertCamRotX = isInvertX;
    }

    public void ToggleInvertY(bool isInvertY)
    {
        SettingsData.InvertCamRotY = isInvertY;
    }

    public void ToggleInvertZoom(bool isInvertZoom)
    {
        SettingsData.InvertScroll = isInvertZoom;
    }

    public void ToggleInvertDrag(bool isInvertDrag)
    {
        SettingsData.InvertDrag = isInvertDrag;
    }

    public void ToggleMouseLock(bool isMouseLock)
    {
        SettingsData.LockMouseOnCameraMovement = isMouseLock;
    }

    public void ToggleNodeColouring(bool isNodeColouring)
    {
        SettingsData.RecolourNodesOnPositioning = isNodeColouring;
    }

    public void ToggleCameraMovement(bool isCameraMovement)
    {
        SettingsData.MoveCamOnSelection = isCameraMovement;
    }
}