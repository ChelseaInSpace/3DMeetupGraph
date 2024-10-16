using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public GameObject MainButtonPanel;
    
    public void LoadSceneFresh()
    {
        SaveLoadManager.ID = Guid.NewGuid().ToString();
        SceneManager.LoadScene("MainScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}