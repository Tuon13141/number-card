using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LayerElement : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Data")]
    private List<StackElement> _stackElements = new List<StackElement>();
    private LayerData _layerData;
    public LayerData layerData => _layerData;
    private int _layerIndex;

    [Header("Settings")]
    [SerializeField] private Vector2 m_StackElementOffset = new Vector2(1, 1.5f);

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void SetUp(LayerData layerData, int layerIndex)
    {
        _layerData = layerData;
        _layerIndex = layerIndex;

        int width = layerData.width;
        int height = layerData.height;
        int total = width * height;

        // Tính offset để tâm layer nằm ngay transform.position
        Vector3 centerOffset = new Vector3(
            (width - 1) * 0.5f * m_StackElementOffset.x,
            (height - 1) * 0.5f * m_StackElementOffset.y,
            0f
        );

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                if (index >= layerData.stackDatas.Count)
                    continue;

                StackElement stackElement = PoolingManager.Instance.Get<StackElement>();

                Vector3 localPos = new Vector3(
                    x * m_StackElementOffset.x,
                    y * m_StackElementOffset.y,
                    0f
                ) - centerOffset;

                stackElement.transform.SetParent(transform, false);
                stackElement.transform.localPosition = localPos;

                _stackElements.Add(stackElement);

                stackElement.SetUp(layerData.stackDatas[index], layerIndex, new Vector2Int(x, y));
            }
        }
    }

    public void SetUpperStack(LayerElement upperLayer)
    {
        foreach(StackElement stackElement in _stackElements)
        {
            int width = layerData.width;
            int height = layerData.height;  

            int upperWidth = upperLayer.layerData.width;
            int upperHeight = upperLayer.layerData.height;

            Vector2Int vector2Int = stackElement.gridPosition;

            List<Vector2Int> upperPositions = MatrixHelper.GetOverlappingCells(width, height, upperWidth, upperHeight, vector2Int);

            List<StackElement> upperStacks = new List<StackElement>();

            foreach(Vector2Int vector in upperPositions)
            {
                StackElement se = upperLayer.GetStackElement(vector);

                upperStacks.Add(se);
            }

            stackElement.SetUpperStack(upperStacks); 

            foreach(StackElement se in upperStacks)
            {
                se.AddLowerStack(stackElement);
            }
        }
    }

    public StackElement GetStackElement(Vector2Int vector2Int)
    {
        foreach (StackElement stackElement in _stackElements)
        {
            if(stackElement.gridPosition == vector2Int) return stackElement;
        }

        return null;
    }

    public void OnReset()
    {
        this.transform.parent = null;

        _layerData = null;
        _layerIndex = -1;

        foreach(StackElement stackElement in _stackElements)
        {
            stackElement.OnReset();
        }

        _stackElements.Clear();

        PoolingManager.Instance.Release(this);
    }
}
