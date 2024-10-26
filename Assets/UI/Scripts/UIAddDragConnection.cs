using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAddDragConnection : MonoBehaviour
{
    public TMP_Text myText;
    Node A;
    Node B;

    public void Initialise(Node a, Node b)
    {
        A = a;
        B = b;
        myText.text = "Create Connection between\n" + a.GetNodeName() + " and " + b.GetNodeName() + "?";
    }

    public void ConfirmConnectionCreation()
    {
        ConnectionHandler.AddConnection(A, B);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        B.MyLine.enabled = false;
    }
}