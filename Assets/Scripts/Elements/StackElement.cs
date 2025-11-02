using System.Collections.Generic;
using UnityEngine;

public class StackElement : MonoBehaviour
{
    [Header("Data")]
    private List<CardElement> _cards = new List<CardElement>();
    public List<CardElement> cards => _cards;

    private StackData _data;

    private int _layer;
    private int _cardCount;
    public int cardCount => _cardCount;

    private Vector2Int _gridPosition;
    public Vector2Int gridPosition => _gridPosition;

    [Header("References")]
    [SerializeField] private StackCounter m_StackCounter;

    List<StackElement> _upperStacks = new List<StackElement>();
    List<StackElement> _lowerStacks = new List<StackElement>();

    public void SetUp(StackData stackData, int layer, Vector2Int gridPosition)
    {
        _data = stackData;
        _layer = layer;
        _gridPosition = gridPosition;
        _cardCount = _data.cardDatas.Count;

        for (int i = 0; i < _cardCount; i++)
        {
            SpawnCardElement(_data.cardDatas[i], i);
            cards[i].gameObject.SetActive(false);
            GameManager.totalCard++;
        }

        if (_cardCount > 0)
        {
            cards[0].gameObject.SetActive(true);
        }

        m_StackCounter.SetText(_cardCount);
    }

    void SpawnCardElement(CardData cardData, int index)
    {
        int type = cardData.type;
        int value = cardData.value;
        Vector2 postion = transform.position;

        switch (type)
        {
            case 0:
                CardElement cardElement = PoolingManager.Instance.Get<CardElement>();
                cardElement.transform.parent = transform;

                cardElement.SetUp(value, _layer, index, postion, this);
                _cards.Add(cardElement);
                break;
            case 1:

                break;
            case 2:

                break;
            default:

                break;
        }
    }

    public void OnCardMerge(CardElement cardElement)
    {
        int i = cards.IndexOf(cardElement);
        cardElement.gameObject.SetActive(false);

        GameManager.totalCard--;
        GameManager.Instance.CheckWin();

        _cardCount--;
        m_StackCounter.SetText(_cardCount);

        if (i >= cards.Count - 1)
        {
            CheckLowerStack();

            return;
        }

        cards[i + 1].gameObject.SetActive(true);


    }

    public bool CheckUpperStack()
    {
        bool b = true;
        foreach (StackElement stackElement in _upperStacks)
        {
            if(stackElement.cardCount > 0)
            {
                b = false;

                break;
            }
        }

        return b;
    }

    public void CheckLowerStack()
    {
        foreach (StackElement stackElement in _lowerStacks)
        {
            if (stackElement.CheckUpperStack())
            {
                stackElement.UnlockCard();
            }
        }
    }

    public void LockCard()
    {
        foreach (CardElement cardElement in cards)
        {
            cardElement.Lock();
        }
    }

    public void UnlockCard()
    {
        foreach(CardElement cardElement in _cards)
        {
            cardElement.Unlock();
        }
    }

    public void SetUpperStack(List<StackElement> upperElements)
    {
        _upperStacks.AddRange(upperElements);

        LockCard();
    }

    public void AddLowerStack(StackElement stackElement)
    {
        _lowerStacks.Add(stackElement);
    }

    public void OnReset()
    {
        this.transform.parent = null;
        
        _data = null;
        _layer = -1;
        _gridPosition = Vector2Int.zero;
        _cardCount = 0;

        m_StackCounter.SetText(0);

        foreach (CardElement card in _cards)
        {
            card.OnReset();
        }

        _cards.Clear();
        _upperStacks.Clear();
        _lowerStacks.Clear();

        PoolingManager.Instance.Release(this);
    }
}
