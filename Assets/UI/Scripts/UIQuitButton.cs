using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIQuitButton : MonoBehaviour
{
    public GameObject QuitDialogue;

    public void OnClick()
    {
        if(SaveLoadManager.DataExists())
            ConfirmQuit();
        else
            QuitDialogue.SetActive(!QuitDialogue.activeSelf);
    }

    public void ConfirmQuit()
    {
        NodeHandler.ClearAllNodes();
        ConnectionHandler.ClearConnections();
        SceneManager.LoadScene("MainMenu");
    }

    public void Save()
    {
        SaveLoadManager.SaveData();
        ConfirmQuit();
    }
}