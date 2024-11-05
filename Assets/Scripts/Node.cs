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
    public Material GlowMaterial;

    Color myColour;
    Material myMaterial;
    string myName = "Empty";
    int dragCounter = 0;
    bool dragging;
    Vector3 worldPosition;

    void Update()
    {
        MyText.transform.rotation = CameraControl.GetCameraTransform().rotation; 
    }

    void FixedUpdate()
    {
        if (dragging)
            dragCounter++;
    }

    public void Initialise(string NodeName, Color c)
    {
        myName = NodeName;
        MyText.text = myName;
        MyText.color = c;
        myColour = c;
        myMaterial = MyRenderer.material;
        SetInactive();
    }

    public string GetNodeName()
    {
        return myName;
    }

    private void OnMouseDrag()
    {
        //TODO: add function to move Nodes around via drag
        if (NodeHandler.GetCurrentNode() == this)
        {
            dragging = true;
            if (dragCounter > 15)
            {
                MyLine.enabled = true;
                MyLine.SetPosition(0, transform.position);
                float distance;
                Plane plane = new(-CameraControl.GetCameraTransform().forward, transform.position);
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
        dragging = false;
        dragCounter = 0;
    }

    public void SetActive()
    {
        MyRenderer.material = GlowMaterial;
        MyRenderer.material.SetColor("_Color", myColour);
    }
    
    public void SetInactive()
    {
        MyRenderer.material = myMaterial;
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
        SetInactive();
        NodeHandler.RedrawNodeInfo();
    }
}
