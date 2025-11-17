using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.GPUSort;

public class LayerElement : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Data")]
    private List<CardElement> _cardElements = new List<CardElement>();
    public List<CardElement> cardElements => _cardElements;

    private LayerData _layerData;
    public LayerData layerData => _layerData;

    private int _layerIndex;
    public int layerIndex => _layerIndex;

    [Header("Settings")]
    [SerializeField] private Vector2 m_CardElementOffset = new Vector2(1, 1.5f);

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

  
        Vector3 centerOffset = new Vector3(
            (width - 1) * 0.5f * m_CardElementOffset.x,
            (height - 1) * 0.5f * m_CardElementOffset.y,
            0f
        );

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                if (index >= layerData.cardDatas.Count)
                    continue;

                Vector3 localPos = new Vector3(
                     x * m_CardElementOffset.x,
                     y * m_CardElementOffset.y,
                     0f
                ) - centerOffset;

                for (int i = 0; i < layerData.cardDatas.Count; i++)
                {
                    SpawnCardElement(layerData.cardDatas[i], i, localPos);
                    
                }
            }
        }
    }

    void SpawnCardElement(CardData cardData, int index, Vector3 localPos)
    {
        int type = cardData.type;
        int value = cardData.value;

        switch (type)
        {
            case 0:
                CardElement cardElement = PoolingManager.Instance.Get<CardElement>();
                cardElement.transform.parent = transform;

                cardElement.SetUp(cardData, localPos, this);
                _cardElements.Add(cardElement);
                GameManager.totalCard++;
                break;
            case 1:
                GameManager.totalCard++;
                break;
            case 2:
                GameManager.totalCard++;
                break;
            default:

                break;
        }
    }


    public void SetUpperCard(LayerElement upperLayer)
    {
        foreach(CardElement cardElement in _cardElements)
        {
            int width = layerData.width;
            int height = layerData.height;  

            int upperWidth = upperLayer.layerData.width;
            int upperHeight = upperLayer.layerData.height;

            Vector2Int vector2Int = cardElement.gridPosition;

            List<Vector2Int> upperPositions = MatrixHelper.GetOverlappingCells(width, height, upperWidth, upperHeight, vector2Int);

            List<CardElement> upperStacks = new List<CardElement>();

            foreach(Vector2Int vector in upperPositions)
            {
                CardElement se = upperLayer.GetCardElement(vector);

                upperStacks.Add(se);
            }

            cardElement.SetUpperCard(upperStacks); 

            foreach(CardElement se in upperStacks)
            {
                se.AddLowerStack(cardElement);
            }
        }
    }

    public CardElement GetCardElement(Vector2Int vector2Int)
    {
        foreach (CardElement cardElement in _cardElements)
        {
            if(cardElement.gridPosition == vector2Int) return cardElement;
        }

        return null;
    }

    public void OnCardMerge(CardElement cardElement)
    {
        int i = cardElements.IndexOf(cardElement);
        cardElement.gameObject.SetActive(false);

        GameManager.totalCard--;
        GameManager.Instance.CheckWin();

        cardElement.CheckLowerStack();

        cardElement.gameObject.SetActive(true);
    }


    public void OnReset()
    {
        this.transform.parent = null;

        _layerData = null;
        _layerIndex = -1;

        foreach(CardElement stackElement in _cardElements)
        {
            stackElement.OnReset();
        }

        _cardElements.Clear();

        PoolingManager.Instance.Release(this);
    }
}
