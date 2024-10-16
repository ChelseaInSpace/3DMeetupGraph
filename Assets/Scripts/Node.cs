using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour
{
    public TextMeshPro MyText;
    public MeshRenderer MyRenderer;
    Color myColour;
    string myName = "Empty";

    void Update()
    {
        MyText.transform.LookAt(CameraControl.GetCameraTransform());
    }

    public void Initialise(string NodeName, Color c)
    {
        myName = NodeName;
        MyText.text = myName;
        MyText.color = c;
        myColour = c;
        ResetColour();
    }

    public string GetNodeName()
    {
        return myName;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            NodeHandler.UpdateCurrentNode(this);
        }
    }

    public void SetActiveColour(Color colour)
    {
        MyRenderer.material.color = colour;
    }
    
    public void ResetColour()
    {
        MyRenderer.material.color = myColour;
    }

    public Color GetColour()
    {
        return myColour;
    }

    public void SetColour(Color c)
    {
        MyText.color = c;
        myColour = c;
        ResetColour();
        NodeHandler.RedrawNodeInfo();
    }
}
