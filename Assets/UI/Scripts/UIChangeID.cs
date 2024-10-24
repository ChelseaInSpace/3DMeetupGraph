using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChangeID : MonoBehaviour
{
    public TMP_InputField MyInput;
    public UIButtonID MyButton;

    public void ConfirmIDChange()
    {
        if (MyInput.text != "")
        {
            SaveLoadManager.ID = MyInput.text;
            MyButton.RefreshText();
            gameObject.SetActive(false);
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
