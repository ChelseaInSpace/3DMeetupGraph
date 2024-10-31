using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public UIOptionsMenu OptionsPanelPrefab;
    UIOptionsMenu myOptionsPanel;
    
    public void LoadSceneFresh()
    {
        SaveLoadManager.ID = Guid.NewGuid().ToString();
        SceneManager.LoadScene("MainScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        if (!myOptionsPanel)
            myOptionsPanel = Instantiate(OptionsPanelPrefab, transform);
        else
            myOptionsPanel.gameObject.SetActive(true);
    }
}