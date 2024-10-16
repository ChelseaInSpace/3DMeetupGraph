using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAddNodeMenu : MonoBehaviour
{
    public TMP_InputField MyInput;
    public Node NodePrefab;
    public Transform Handler;
    Color myColor = Color.black;
    Button[] buttons;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void CreateNode()
    {
        NodeHandler.CreateNode(MyInput.text, myColor);
        this.gameObject.SetActive(false);
    }

    public void OnConfirm()
    {
        if(MyInput.text=="")
        {
            Debug.Log("Empty Field!");
        }
        else
        {
            Node a = NodeHandler.NodeList.Find(n => n.GetNodeName() == MyInput.text);
            if (a != null)
            {
                Debug.Log("node already exists!");
            }
            else
            {
                CreateNode();
            }
        }
    }

    public void SetColor(Button b)
    {
        foreach(Button btn in buttons)
        {
            btn.interactable = true;
        }
        myColor = b.image.color;
        b.interactable = false;
    }

    public void OnDisable()
    {
        myColor = Color.black;
        MyInput.text = "";
        foreach (Button btn in buttons)
        {
            btn.interactable = true;
        }
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
