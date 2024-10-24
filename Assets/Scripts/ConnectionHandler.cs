using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    public struct Connection
    {

        public Connection(Node a, Node b)
        {
            A = a;
            B = b;
        }

        public Node A { get; }
        public Node B { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() == this.GetType())
            {
                Connection c = (Connection)obj;

                if(c.A == this.A && c.B == this.B)
                {
                    return true;
                }
                else if(c.B == this.A && c.A == this.B)
                {
                    return true;
                }
                else
                {
                    return false;
                }    
            }
            else
            {
                return false;
            }
        }
    }

    public static List<Connection> ConnectionList = new List<Connection>();
    public static List<ConnectionDisplay> ConnectionDisplays = new List<ConnectionDisplay>();
    public ConnectionDisplay Prefab;
    public static ConnectionDisplay CD;
    public static Transform me;
    public UICurrentNode NodeDisplayReference;
    static UICurrentNode CurrentNodeDisplay;

    void Awake()
    {
        CurrentNodeDisplay = NodeDisplayReference;
        me = gameObject.transform;
        CD = Prefab;
    }

    public static void AddConnection(Node a, Node b)
    {
        Connection c = new Connection(a, b);
        if(!DoesConnectionExist(c))
        {
            ConnectionList.Add(c);
            RenderConnections();
            CurrentNodeDisplay.ListConnections(NodeHandler.GetCurrentNode());
        }
    }

    public static void AddConnection(string a, string b)
    {
        Node nodeA = NodeHandler.NodeList.Find(n => n.GetNodeName() == a);
        Node nodeB = NodeHandler.NodeList.Find(n => n.GetNodeName() == b);
        if(nodeA != null && nodeB != null)
        {
            AddConnection(nodeA, nodeB);
        }
        else
        {
            Debug.Log("couldn't parse string connection input for: " + a + ", " + b + ".");
        }
    }

    public static void CreateConnectionFromDrag(Node a, Node b)
    {
        AddConnection(a, b);
    }

    public static bool DoesConnectionExist(Connection c)
    {
        bool exists = false;
        foreach(Connection co in ConnectionList)
        {
            if (co.Equals(c))
            {
                exists = true;
                Debug.Log("Connection already exists: " + c.A.GetNodeName() + ", " + c.B.GetNodeName() + ".");
            }
        }
        return exists;
    }

    public static void RenderConnections()
    {
        foreach (ConnectionDisplay d in ConnectionDisplays)
        {
            if(d != null)
                Destroy(d.gameObject);
        }
        ConnectionDisplays.Clear();

        foreach(Connection c in ConnectionList)
        {
            ConnectionDisplay cd = Instantiate(CD, me);
            if(c.A == null || c.B == null)
            {
                Debug.Log("Node A or Node B missing in connection");
            }
            Vector3 middle = (c.A.transform.position + c.B.transform.position) / 2.0f;
            float distance = Vector3.Distance(c.A.transform.position, c.B.transform.position);
            cd.transform.position = middle;
            Vector3 direction = (c.A.transform.position - c.B.transform.position).normalized;
            cd.transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(90, 0, 0); 
            cd.transform.localScale += new Vector3(0, (distance / 2)-1, 0);

            cd.GetComponent<Renderer>().material.SetColor("_ColourTop", c.A.GetColour());
            cd.GetComponent<Renderer>().material.SetColor("_ColourBottom", c.B.GetColour());
            ConnectionDisplays.Add(cd);
        }
    }

    public static void ClearConnections()
    {
        ConnectionList.Clear();
    }

    public static void RemoveConnection(Node A, Node B)
    {
        Connection temp = new Connection(A, B);
        foreach(Connection c in ConnectionList)
        {
            if(c.Equals(temp))
            {
                temp = c;
            }
        }
        ConnectionList.Remove(temp);
        RenderConnections();
    }

    public static void RemoveConnectionsForNode(Node n)
    {
        ConnectionList.RemoveAll(c => c.A == n || c.B == n);
    }

    public static List<Connection> ListAllConnectionsForNode(Node n)
    {
        List<Connection> temp = new List<Connection>();
        temp = ConnectionList.FindAll(c => c.A == n || c.B == n);
        return temp;
    }

    public static int GetAmountOfTotalConnectionsForPartnersOfConnection(Connection c)
    {
        int i = 0;
        foreach(Connection co in ConnectionList)
        {
            if (c.A == co.A || c.A == co.B)
                i += 1;
            if(c.B == co.A || c.B == co.B)
                i += 1;
        }
        return i;
    }
}