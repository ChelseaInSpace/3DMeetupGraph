using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    [System.Serializable]
    public class NodeData
    {
        public string Name;
        public Color Colour;
    }

    [System.Serializable]
    public class ConnectionsData
    {
        public string NodeA;
        public string NodeB;
    }

    [System.Serializable]
    public class NDList
    {
        public List<NodeData> Nodes;

        public NDList(List<NodeData> list)
        {
            Nodes = list;
        }

        public void Add(NodeData nd)
        {
            Nodes.Add(nd);
        }
    }

    [System.Serializable]
    public class CDList
    {
        public List<ConnectionsData> Connections;

        public CDList(List<ConnectionsData> list)
        {
            Connections = list;
        }

        public void Add(ConnectionsData cd)
        {
            Connections.Add(cd);
        }
    }

    public static bool Load;
    public static string ID = "Default";


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void SaveData()
    {
        string path = Application.persistentDataPath + "/" + ID;
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        NDList ndlist = new NDList(new List<NodeData>());
        foreach (Node n in NodeHandler.NodeList)
        {
            NodeData nd = new NodeData
            {
                Name = n.GetNodeName(),
                Colour = n.GetColour()
            };
            ndlist.Add(nd);
        }
        File.WriteAllText(path + "/Nodes.json", JsonUtility.ToJson(ndlist));

        CDList cdlist = new CDList(new List<ConnectionsData>());
        foreach (ConnectionHandler.Connection c in ConnectionHandler.ConnectionList)
        {
            ConnectionsData cd = new ConnectionsData
            {
                NodeA = c.A.GetNodeName(),
                NodeB = c.B.GetNodeName()
            };
            cdlist.Add(cd);
        }
        File.WriteAllText(path + "/Connections.json", JsonUtility.ToJson(cdlist));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(Load)
        {
            LoadData(ID);
            Load = false;
        }
    }

    public static void LoadData(string id)
    {
        NDList LoadedND = new NDList(new List<NodeData>());
        CDList LoadedCD = new CDList(new List<ConnectionsData>());

        string path = Application.persistentDataPath + "/" + id;
        if (System.IO.File.Exists(path + "/Nodes.json"))
        {     
            string nodes = File.ReadAllText(path + "/Nodes.json");
            LoadedND = JsonUtility.FromJson<NDList>(nodes);
            foreach(NodeData nd in LoadedND.Nodes)
            {
                if (nd.Colour == Color.white)
                    NodeHandler.CreateNode(nd.Name, ColourPicker.RandomColourFromPool());
                else
                    NodeHandler.CreateNode(nd.Name, nd.Colour);
            }
        }
        
        if (System.IO.File.Exists(path + "/Connections.json"))
        {
            string connections = File.ReadAllText(path + "/Connections.json");
            LoadedCD = JsonUtility.FromJson<CDList>(connections);

            foreach(ConnectionsData cd in LoadedCD.Connections)
            {
                if (!NodeHandler.HasNodeOfName(cd.NodeA))
                {
                    NodeHandler.CreateNode(cd.NodeA, ColourPicker.RandomColourFromPool());
                }
                if (!NodeHandler.HasNodeOfName(cd.NodeB))
                {
                    NodeHandler.CreateNode(cd.NodeB, ColourPicker.RandomColourFromPool());
                }
                ConnectionHandler.AddConnection(cd.NodeA, cd.NodeB);
            }
        }
    }
}