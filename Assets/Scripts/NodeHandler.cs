using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrincipalComponent;
using System;
using System.Linq;

public class NodeHandler : MonoBehaviour
{
    public CameraControl MyCameraReference;
    static CameraControl MyCamera;
    public Node Prefab;
    static Node NodePrefab;
    public static List<Node> NodeList = new List<Node>();
    public UICurrentNode NodeDisplayReference;
    static UICurrentNode CurrentNodeDisplay;
    public UINodeList NodeListReference;
    static UINodeList NodeListDisplay;
    
    static Node currentNode;
    static Transform me;
    static double[] values;
    static double[][] vectors;
    static List<Node> discardedNodes = new List<Node>();
    static List<ConnectionHandler.Connection> discardedConnections = new List<ConnectionHandler.Connection>();

    void Awake()
    {
        MyCamera = MyCameraReference;
        NodePrefab = Prefab;
        me = transform;
        NodeListDisplay = NodeListReference;
        CurrentNodeDisplay = NodeDisplayReference;
    }

    public static void AddNode(Node newNode)
    {
        NodeList.Add(newNode);
        NodeListDisplay.InitialiseList();
    }

    public static void CreateNode(string name, Color colour)
    {
        if(NodePrefab != null)
        {
            Node newNode = Instantiate(NodePrefab, me);
            newNode.transform.position = new Vector3(UnityEngine.Random.Range(-3.0f, 3.0f), UnityEngine.Random.Range(-3.0f, 3.0f), UnityEngine.Random.Range(-3.0f, 3.0f));
            newNode.Initialise(name, colour);
            AddNode(newNode);
        }
    }

    public static void UpdateCurrentNode(Node newCurrentNode)
    {
        if (currentNode != newCurrentNode)
        {
            if (currentNode)
                currentNode.SetInactive();
            currentNode = newCurrentNode;
            currentNode.SetActive();
            MyCamera.SetCameraTarget(currentNode.transform.position);
            CurrentNodeDisplay.Fill(currentNode);
        }
    }

    public static void SetNoCurrentNode()
    {
        if (currentNode)
        {
            currentNode.SetInactive();
            currentNode = null;
            CurrentNodeDisplay.Empty();
        }
    }

    public static bool HasCurrentNode()
    {
        return currentNode != null;
    }

    public static Node GetCurrentNode()
    {
        return currentNode;
    }

    public static void DeleteNode()
    {
        if (HasCurrentNode())
        {
            ConnectionHandler.RemoveConnectionsForNode(currentNode);
            ConnectionHandler.RenderConnections();
            NodeList.Remove(currentNode);
            Destroy(currentNode.gameObject);
            NodeListDisplay.InitialiseList();
            currentNode = null;
            CurrentNodeDisplay.Empty();
        }
    }

    public static void RedrawNodeInfo()
    {
        NodeListDisplay.InitialiseList();
        if (currentNode != null)
            CurrentNodeDisplay.Fill(currentNode);
        ConnectionHandler.RenderConnections();
    }

    public static void ClearAllNodes()
    {
        NodeList.Clear();
        currentNode = null;
    }

    public static bool HasNodeOfName(string name)
    {
        return NodeList.Any(n => n.GetNodeName() == name);
    }

    public static void CalculatePositions()
    {    
        for (int i = 0; i < NodeList.Count; i++)
        {
            Node n = NodeList[i];
            double[] eigenVectorOfPersonI = vectors[i];
            Vector3 pos = new Vector3((float)eigenVectorOfPersonI[eigenVectorOfPersonI.Length - 2] * 7, (float)eigenVectorOfPersonI[eigenVectorOfPersonI.Length - 3] * 7, (float)eigenVectorOfPersonI[eigenVectorOfPersonI.Length - 4] * 7);
            n.transform.position = pos;
        }
        discardedNodes.Reverse();
        foreach(Node node in discardedNodes)
        {
            foreach(ConnectionHandler.Connection c in discardedConnections)
            {
                if(c.A == node)
                {
                    if(NodeList.Contains(c.B))
                    {
                        node.transform.position = c.B.transform.position * 1.4f;
                        NodeList.Add(node);
                    }
                }
                if(c.B == node)
                {
                    if(NodeList.Contains(c.A))
                    {
                        node.transform.position = c.A.transform.position * 1.4f;
                        NodeList.Add(node);
                    }
                }
            }
        }
        NodeList = NodeList.Union(discardedNodes).ToList();
        ConnectionHandler.ConnectionList.AddRange(discardedConnections);
        FixOverlaps();
        if(SettingsData.RecolourNodesOnPositioning)
            RainbowHSVColours();
        discardedConnections.Clear();
        discardedNodes.Clear();
        ConnectionHandler.RenderConnections();
    }

    public static void CalculateEigenVectorsValues()
    {
        discardedNodes = SeparateSingles();
        float[,] matrix = CreateMatrix();
        double[][] myMatrix = new double[(int)Mathf.Sqrt(matrix.Length)][];
        for (int i = 0; i < Mathf.Sqrt(matrix.Length); i++)
        {
            myMatrix[i] = new double[(int)Mathf.Sqrt(matrix.Length)];

            for (int j = 0; j < Mathf.Sqrt(matrix.Length); j++)
            {
                myMatrix[i][j] = matrix[i, j];
            }
        }
        double[][] eigenVectors;
        double[] eigenValues;
        PrincipalComponentProgram.Eigen(myMatrix, out eigenValues, out eigenVectors);
        values = eigenValues;
        vectors = eigenVectors;
    }

    public static List<Node> SeparateSingles()
    {
        List<Node> singles = new List<Node>();

        foreach (Node n in NodeList)
        {
            if (HasOnlySingleConnection(n))
            {
                discardedConnections.AddRange(ConnectionHandler.ListAllConnectionsForNode(n));
                ConnectionHandler.RemoveConnectionsForNode(n);
                singles.Add(n);
            }
        }
        foreach (Node no in singles)
        {
            NodeList.Remove(no);
        }

        if (NodeList.Any(n => HasOnlySingleConnection(n)))
        {
            singles.AddRange(SeparateSingles());
        }
        return singles;
    }

    public static bool HasOnlySingleConnection(Node n)
    {
        int count = 0;
        foreach (ConnectionHandler.Connection c in ConnectionHandler.ConnectionList)
        {
            if (c.A.GetNodeName() == n.GetNodeName() || c.B.GetNodeName() == n.GetNodeName())
            {
                count++;
            }
        }
        return count <= 1;
    }

    public static float[,] CreateMatrix()
    {
        List<Node> localNodes = NodeList;
        List<ConnectionHandler.Connection> localConnections = ConnectionHandler.ConnectionList;

        float[,] matrix = new float[localNodes.Count, localNodes.Count];

        foreach (ConnectionHandler.Connection c in localConnections)
        {
            int i = localNodes.IndexOf(c.A);
            int j = localNodes.IndexOf(c.B);

            int cWeight = ConnectionHandler.GetAmountOfTotalConnectionsForPartnersOfConnection(c);
            cWeight /= 2;
            float value = 1f;
            value -= (cWeight * 0.05f);
            if (value <= 0)
            {
                value = 0.05f;
            }
            for (int x = 1; x <= cWeight; x++)
            {
                value -= Mathf.Pow(0.2f, x);
            }

            matrix[i, j] = -value;
            matrix[j, i] = -value;
            matrix[j, j] += value;
            matrix[i, i] += value;
        }
        return matrix;
    }

    public static void FixOverlaps()
    {
        foreach (Node n in NodeList)
        {
            foreach (Node no in NodeList)
            {
                if (n != no)
                {
                    if (n.transform.position == no.transform.position)
                    {
                        no.transform.position = new Vector3(no.transform.position.x + 0.1f, no.transform.position.y - 0.15f, no.transform.position.z * 1.1f);
                    }
                }
            }
        }
    }

    public static void RainbowHSVColours()
    {
        List<Node> temp = NodeList.OrderBy(n => n.transform.position.y).ToList();
        float fraction = 1f / NodeList.Count;
        for(int i = 0; i < temp.Count; i++)
        {
            temp[i].SetColour(Color.HSVToRGB(fraction * i, 0.75f, 0.75f));
        }
    }

    //public static void ClampPosition(Node n)
    //{
    //    float newX = n.transform.position.x;
    //    while (Math.Abs(newX) > 2.5)
    //    {
    //        newX /= 1.5f;
    //    }
    //    float newY = n.transform.position.y;
    //    while (Math.Abs(newY) > 2.5)
    //    {
    //        newY /= 1.5f;
    //    }
    //    float newZ = n.transform.position.z;
    //    while (Math.Abs(newZ) > 2.5)
    //    {
    //        newZ /= 1.5f;
    //    }
    //    n.transform.position = new Vector3(newX, newY, newZ);
    //}

    // public static Color RGBFromMatrix(double[][] vectors, int nodeIndex)
    // {
    //     double minimumRed = 9999;
    //     double maximumRed = -9999;
    //     for (int i = 0; i < vectors.Length; i++)
    //     {
    //         if(vectors[i][vectors.Length-5] < minimumRed)
    //             minimumRed = vectors[i][vectors.Length - 5];
    //         if (vectors[i][vectors.Length - 5] > maximumRed)
    //             maximumRed = vectors[i][vectors.Length - 5];
    //     }
    //     float tempRed = (float) ((vectors[nodeIndex][vectors.Length-5] - minimumRed) / (maximumRed - minimumRed));
    //
    //     double minimumGreen = 9999;
    //     double maximumGreen = -9999;
    //     for(int i = 0; i < vectors.Length; i++)
    //     {
    //         if (vectors[i][vectors.Length - 6] < minimumGreen)
    //             minimumGreen = vectors[i][vectors.Length - 6];
    //         if (vectors[i][vectors.Length - 6] > maximumGreen)
    //             maximumGreen = vectors[i][vectors.Length - 6];
    //     }
    //     float tempGreen = (float)((vectors[nodeIndex][vectors.Length - 6] - minimumGreen) / (maximumGreen - minimumGreen));
    //
    //     double minimumBlue = 9999;
    //     double maximumBlue = -9999;
    //     for (int i = 0; i < vectors.Length; i++)
    //     {
    //         if (vectors[i][vectors.Length - 7] < minimumBlue)
    //             minimumBlue = vectors[i][vectors.Length - 7];
    //         if (vectors[i][vectors.Length - 7] > maximumBlue)
    //             maximumBlue = vectors[i][vectors.Length - 7];
    //     }
    //     float tempBlue = (float)((vectors[nodeIndex][vectors.Length - 7] - minimumBlue) / (maximumBlue - minimumBlue));
    //
    //     return new Color(tempRed, tempGreen, tempBlue);
    // }
}