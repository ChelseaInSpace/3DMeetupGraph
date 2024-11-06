using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UILoadMenu : MonoBehaviour
{
    public Transform Viewport;
    public UIButtonProjectLoad ButtonPrefab;

    UIButtonProjectLoad highlightedButton = null;
    List<UIButtonProjectLoad> btns;

    void Awake()
    {
        string[] savedGraphs = Directory.GetDirectories(Application.persistentDataPath);

        btns = new List<UIButtonProjectLoad>();

        foreach (string s in savedGraphs)
        {
            UIButtonProjectLoad temp = Instantiate(ButtonPrefab, Viewport);
            btns.Add(temp);
            temp.ID = Path.GetFileName(s);
            temp.MyText.text = temp.ID;
            temp.GetComponent<Button>().onClick.AddListener(() => Highlight(temp));
        }
    }

    public void LoadSceneWithData(string id)
    {
        SaveLoadManager.ID = id;
        SaveLoadManager.Load = true;
        SceneManager.LoadScene("MainScene");
    }

    public void Highlight(UIButtonProjectLoad btn)
    {
        if (highlightedButton == btn)
        {
            LoadSceneWithData(highlightedButton.ID);
        }
        else
        {
            highlightedButton = btn;
            foreach (UIButtonProjectLoad b in btns)
            {
                b.UnhighlightEffect();
            }
            highlightedButton.HighlightEffect();
        }
    }

    public void ConfirmLoad()
    {
        if(highlightedButton != null)
        {
            LoadSceneWithData(highlightedButton.ID);
        }
    }

    public void DeleteProject()
    {
        if (highlightedButton != null)
        {
            Directory.Delete(Application.persistentDataPath + "/" + highlightedButton.ID, true);
            btns.Remove(highlightedButton);
            Destroy(highlightedButton.gameObject);
        }
    }
}