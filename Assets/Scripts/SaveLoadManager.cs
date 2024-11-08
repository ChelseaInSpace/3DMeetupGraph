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
    public class PositionData
    {
        public string Name;
        public Vector3 Position;
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

    [System.Serializable]
    public class PDList
    {
        public List<PositionData> Positions;

        public PDList(List<PositionData> list)
        {
            Positions = list;
        }

        public void Add(PositionData pd)
        {
            Positions.Add(pd);
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
        File.WriteAllText(path + "/Nodes.json", JsonUtility.ToJson(NodesToFile()));
        File.WriteAllText(path + "/Connections.json", JsonUtility.ToJson(ConnectionsToFile()));
        File.WriteAllText(path + "/Positions.json", JsonUtility.ToJson(PositionsToFile()));
    }

    public static NDList NodesToFile()
    {
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
        return ndlist;
    }

    public static CDList ConnectionsToFile()
    {
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
        return cdlist;
    }

    public static PDList PositionsToFile()
    {
        PDList pdlist = new PDList(new List<PositionData>());
        foreach(Node n in NodeHandler.NodeList)
        {
            PositionData pd = new PositionData
            {
                Name = n.GetNodeName(),
                Position = n.transform.position
            };
            pdlist.Add(pd);
        }
        return pdlist;
    }

    public static bool DataExists()
    {
        string pathNodes = Application.persistentDataPath + "/" + ID + "/Nodes.json";
        string pathConnections = Application.persistentDataPath + "/" + ID + "/Connections.json";
        string pathPositions = Application.persistentDataPath + "/" + ID + "/Positions.json";
        if (!File.Exists(pathNodes))
        {
            return false;
        }
        if (!File.Exists(pathConnections))
        {
            return false;
        }
        if(!File.Exists(pathPositions))
        {
            return false;
        }
        return JsonUtility.ToJson(NodesToFile()) == File.ReadAllText(pathNodes) && JsonUtility.ToJson(ConnectionsToFile()) == File.ReadAllText(pathConnections) && JsonUtility.ToJson(PositionsToFile()) == File.ReadAllText(pathPositions);
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
        PDList LoadedPD = new PDList(new List<PositionData>());

        string path = Application.persistentDataPath + "/" + id;
        if (File.Exists(path + "/Nodes.json"))
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

        if (File.Exists(path + "/Positions.json"))
        {
            string positions = File.ReadAllText(path + "/Positions.json");
            LoadedPD = JsonUtility.FromJson<PDList>(positions);
            foreach (PositionData pd in LoadedPD.Positions)
            {
                NodeHandler.NodeList.Find(n => n.GetNodeName() == pd.Name).transform.position = pd.Position;
            }
        }

        if (File.Exists(path + "/Connections.json"))
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