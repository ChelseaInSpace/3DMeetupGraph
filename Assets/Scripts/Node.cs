using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour
{
    public TextMeshPro MyText;
    public MeshRenderer MyRenderer;
    public LineRenderer MyLine;
    Color myColour;
    string myName = "Empty";
    int dragCounter = 0;
    Vector3 worldPosition;
    Plane plane = new(Vector3.forward, 0);

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

    private void OnMouseDrag()
    {
        if (NodeHandler.GetCurrentNode() == this)
        {
            dragCounter++;
            if (dragCounter > 7)
            {
                MyLine.enabled = true;
                MyLine.SetPosition(0, this.transform.position);
                float distance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out distance))
                {
                    worldPosition = ray.GetPoint(distance);
                }
                MyLine.SetPosition(1, worldPosition);
            }
        }
    }

    private void OnMouseUp()
    {
        MyLine.enabled = false;
        if (NodeHandler.GetCurrentNode() == this)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Node otherNode = hit.transform.GetComponent<Node>();
                if (otherNode != null && otherNode != this)
                {
                    MyLine.enabled = true;
                    ConnectionHandler.CreateConnectionFromDrag(otherNode, this);
                }
            }
        }
        dragCounter = 0;
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
