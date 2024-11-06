using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSave : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(SaveOnClick);
    }
    
    void SaveOnClick()
    {
        SaveLoadManager.SaveData();
    }
}