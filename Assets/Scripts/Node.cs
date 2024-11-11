using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour
{
    public TextMeshPro MyText;
    public Transform TextAnchor;
    public MeshRenderer MyRenderer;
    public LineRenderer MyLine;
    public Material GlowMaterial;

    Color myColour;
    Material myMaterial;
    string myName = "Empty";
    bool isActive = false;
    int dragCounter = 0;
    bool dragging;
    bool nodeMoving;
    Vector3 worldPosition;

    void Update()
    {
        TextAnchor.rotation = CameraControl.GetCameraTransform().rotation; 
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
        if(!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            if (NodeHandler.GetCurrentNode() == this && !CameraControl.IsMoving())
            {
                dragging = true;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    MyLine.enabled = false;
                    nodeMoving = true;
                    float distance;
                    Plane plane = new(-CameraControl.GetCameraTransform().forward, transform.position);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (plane.Raycast(ray, out distance))
                    {
                        transform.position = ray.GetPoint(distance);
                        ConnectionHandler.UpdateConnectionsForNode(this);
                    }
                }
                else if (dragCounter > 15)
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
                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                {
                    nodeMoving = false;
                }
            }
        }
    }

    private void OnMouseUp()
    {
        MyLine.enabled = false;
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            if (nodeMoving)
            {
                nodeMoving = false;
            }
            else
            {
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
            }
        }
        dragging = false;
        dragCounter = 0;
    }

    public void SetActive()
    {
        isActive = true;
        MyRenderer.material = GlowMaterial;
        MyRenderer.material.SetColor("_Color", myColour);
    }
    
    public void SetInactive()
    {
        isActive = false;
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
        if (isActive)
            SetActive();
        else
            SetInactive();
        NodeHandler.RedrawNodeInfo();
    }
}
