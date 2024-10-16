using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonProjectLoad : MonoBehaviour
{
    public Button MyButton;
    public TMP_Text MyText;
    public string ID;

    public void HighlightEffect()
    {
        MyButton.image.color = new Color(0.4666667f, 0.4117647f, 0.9803922f, 1);
        MyText.color = Color.white;
    }

    public void UnhighlightEffect()
    {
        MyButton.image.color = Color.clear;
        MyText.color = Color.black;
    }
}
