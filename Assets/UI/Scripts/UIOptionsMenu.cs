using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionsMenu : MonoBehaviour
{
    public void ToggleFullscreen(bool isFullscreen)
    {
        SettingsData.Fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }
}
