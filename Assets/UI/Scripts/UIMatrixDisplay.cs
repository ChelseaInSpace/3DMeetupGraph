using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UIMatrixDisplay : MonoBehaviour
{
    public Transform LoadPanel;
    public RectTransform Circle;

    public async void CalculatePositions()
    {
        if (NodeHandler.NodeList.Count > 3)
        {
            LoadPanel.gameObject.SetActive(true);
            await Task.Run(() => { NodeHandler.CalculateEigenVectorsValues(); });
            NodeHandler.CalculatePositions();
            LoadPanel.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Circle.Rotate(0f, 0f, -200f * Time.deltaTime);
    }
}