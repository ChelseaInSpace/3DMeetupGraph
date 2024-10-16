using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAddConnectionMenu : MonoBehaviour
{
    public TMP_InputField MyInput; 
    public Transform Handler;
    Node target;
    
    public void CreateConnection(Node n)
    {
        ConnectionHandler.AddConnection(target, NodeHandler.GetCurrentNode());
        gameObject.SetActive(false);
    }

    public void OnConfirm()
    {
        if (MyInput.text != "" && MyInput.text != NodeHandler.GetCurrentNode().GetNodeName())
        {
            target = NodeHandler.NodeList.Find(n => n.GetNodeName() == MyInput.text);
            if(target != null)
            {
                CreateConnection(target);
            }
        }
    }

    public void OnDisable()
    {
        MyInput.text = "";
        CameraControl.MovementEnabled = true;
    }
    
    public void OnEnable()
    {
        MyInput.ActivateInputField();
        MyInput.Select();
    }
    public void OnSelect()
    {
        CameraControl.MovementEnabled = false;
    }

    public void OnDeselect()
    {
        CameraControl.MovementEnabled = true;
    }
}