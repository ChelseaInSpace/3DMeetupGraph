using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICurrentNode : MonoBehaviour
{
    public TMP_Text MyText;
    public Transform Parent;
    public Transform Viewport;
    public Button NodeEntryPrefab;
    public Transform ConnectionEntryPrefab;
    private List<Transform> entries = new List<Transform>();
    private Node referencedNode;

    public void Initialise(Node n)
    {
        referencedNode = n;
        MyText.text = referencedNode.GetNodeName();
        MyText.color = n.GetColour();
        ListConnections(n);
        Parent.gameObject.SetActive(true);
    }

    public void Empty()
    {
        MyText.text = "";
        MyText.color = Color.black;
        Parent.gameObject.SetActive(false);
    }

    public void DeleteCurrentNode()
    {
        NodeHandler.DeleteNode();
    }

    public void ListConnections(Node no)
    {
        if (no != null)
        {
            foreach (Transform t in entries)
            {
                if(t!=null)
                    Destroy(t.gameObject);
            }
            entries.Clear();
            foreach (ConnectionHandler.Connection c in ConnectionHandler.ConnectionList)
            {
                if (c.A.GetNodeName() == no.GetNodeName())
                {
                    CreateEntry(c.B);
                }
                if (c.B.GetNodeName() == no.GetNodeName())
                {
                    CreateEntry(c.A);
                }
            }
        }
    }

    public void CreateEntry(Node n)
    {
        Transform t = Instantiate(ConnectionEntryPrefab, Viewport);
        t.GetComponentInChildren<TMP_Text>().text = n.GetNodeName();
        Button[] btns = t.GetComponentsInChildren<Button>();
        entries.Add(t);
        btns[0].image.color = n.GetColour();
        btns[0].onClick.AddListener(delegate () { SetCurrentNode(n); });
        btns[1].onClick.AddListener(delegate () { DeleteConnection(n); });
    }
    void SetCurrentNode(Node n)
    {
        NodeHandler.UpdateCurrentNode(n);
    }

    void DeleteConnection(Node no)
    {
        ConnectionHandler.RemoveConnection(no, NodeHandler.GetCurrentNode());
        ListConnections(NodeHandler.GetCurrentNode());
    }
}