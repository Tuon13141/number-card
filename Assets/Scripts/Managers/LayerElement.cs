using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LayerElement : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Data")]
    private List<StackElement> _stackElements = new List<StackElement>();
    private LayerData _layerData;
    private int _layerIndex;

    [Header("Settings")]
    [SerializeField] private Vector2 m_StackElementOffset = new Vector2(1, 1.5f);
    [SerializeField] private SortingGroup m_SortingGroup;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void SetUp(LayerData layerData, int layerIndex)
    {
        _layerData = layerData;
        _layerIndex = layerIndex;

        m_SortingGroup.sortingOrder = _layerIndex;

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

                stackElement.SetUp(layerData.stackDatas[index], layerIndex);
            }
        }
    }
}
