using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAddNodeButton : MonoBehaviour
{
    public GameObject MyMenu;

    public void ToggleMenu()
    {
        MyMenu.SetActive(!MyMenu.activeSelf);
    }
}
