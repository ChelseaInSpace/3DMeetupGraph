using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChangeColourMenu : MonoBehaviour
{
    public TMP_InputField MyInput;

    public void ConfirmColourChange()
    {
        if(MyInput.text != "")
        {
            Color c;
            if (ColorUtility.TryParseHtmlString(MyInput.text, out c))
            {
                NodeHandler.GetCurrentNode().SetColour(c);
                gameObject.SetActive(false);
            }
            else
                Debug.Log("invalid input");
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
