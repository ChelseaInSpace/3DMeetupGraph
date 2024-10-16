using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPositionNodesButton : MonoBehaviour
{
    public Button PositionButton;

    void Update()
    {
        PositionButton.interactable = NodeHandler.NodeList.Count > 4 && ConnectionHandler.ConnectionList.Count > 5;
    }
}