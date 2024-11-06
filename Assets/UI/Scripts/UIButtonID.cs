using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButtonID : MonoBehaviour
{
    public TMP_Text IDText;

    void Awake()
    {
        RefreshText();
    }

    public void RefreshText()
    {
        IDText.text = SaveLoadManager.ID;
    }
}