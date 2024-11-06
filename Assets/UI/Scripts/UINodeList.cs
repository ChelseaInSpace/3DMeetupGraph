using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UINodeList : MonoBehaviour
{
    public Button NodeEntryPrefab;
    public Transform Viewport;
    List<Button> myButtons;

    void Awake()
    {
        myButtons = new List<Button>();
    }

    public void InitialiseList()
    {
        if(myButtons.Count > 0)
        {
            foreach(Button b in myButtons)
            {
                Destroy(b.gameObject);
            }
            myButtons.Clear();
        }
        foreach(Node n in NodeHandler.NodeList)
        {
            Button b = Instantiate(NodeEntryPrefab, Viewport);
            myButtons.Add(b);
            b.GetComponentInChildren<TMP_Text>().text = n.GetNodeName();
            b.image.color = n.GetColour();
            b.onClick.AddListener(delegate () { SetCurrentNode(n); });
        }
    }

    void SetCurrentNode(Node n)
    {
        NodeHandler.UpdateCurrentNode(n);
    }
}