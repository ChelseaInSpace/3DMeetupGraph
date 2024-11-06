using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollapsePanel : MonoBehaviour
{
    public GameObject MyPanel;
    public RectTransform MyRT;
    Button me;
    bool visible = true;
    Vector2 startingPosition;

    void Start()
    {
        me = gameObject.GetComponent<Button>();
        me.onClick.AddListener(delegate () { CollapsePanel(); });
        startingPosition = MyRT.anchoredPosition;
    }

    void CollapsePanel()
    {
        MyPanel.SetActive(!visible);
        if (visible)
        {
            MyRT.anchoredPosition = new Vector2(0, 0);
        }
        else
            MyRT.anchoredPosition = startingPosition;
        visible = !visible;
    }
}