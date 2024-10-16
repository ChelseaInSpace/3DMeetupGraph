using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSave : MonoBehaviour
{
    // Start is called before the first frame update
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
