using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAddConnectionButton : MonoBehaviour
{
    public GameObject MyMenu;
    Button me;

    void Start()
    {
        me = gameObject.GetComponent<Button>(); 
    }

    void Update()
    {
        me.interactable = NodeHandler.HasCurrentNode();
    }

    public void ToggleMenu()
    {
        MyMenu.SetActive(!MyMenu.activeSelf);
    }
}