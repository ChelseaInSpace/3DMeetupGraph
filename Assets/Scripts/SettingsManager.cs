using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    
    void Start()
    {
        SettingsData.InvertCamRotX = PlayerPrefs.GetInt("InvertCamRotX", 0) != 0;
        SettingsData.InvertCamRotY = PlayerPrefs.GetInt("InvertCamRotY", 1) != 0;
        SettingsData.InvertScroll = PlayerPrefs.GetInt("InvertScroll", 0) != 0;
        SettingsData.InvertDrag = PlayerPrefs.GetInt("InvertDrag", 1) != 0;
        SettingsData.LockMouseOnCameraMovement = PlayerPrefs.GetInt("LockMouseOnCameraMovement", 0) != 0;
        SettingsData.RecolourNodesOnPositioning = PlayerPrefs.GetInt("RecolourNodesOnPositioning", 1) != 0;
    }

    public static void SaveSettings()
    {
        PlayerPrefs.SetInt("InvertDrag", SettingsData.InvertDrag ? 1 : 0);
        PlayerPrefs.SetInt("InvertScroll", SettingsData.InvertScroll ? 1 : 0);
        PlayerPrefs.SetInt("InvertCamRotX", SettingsData.InvertCamRotX ? 1 : 0);
        PlayerPrefs.SetInt("InvertCamRotY", SettingsData.InvertCamRotY ? 1 : 0);
        PlayerPrefs.SetInt("LockMouseOnCameraMovement", SettingsData.LockMouseOnCameraMovement ? 1 : 0);
        PlayerPrefs.SetInt("RecolourNodesOnPositioning", SettingsData.RecolourNodesOnPositioning ? 1 : 0);
    }
}