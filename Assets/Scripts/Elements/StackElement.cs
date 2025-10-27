using System.Collections.Generic;
using UnityEngine;

public class StackElement : MonoBehaviour
{
    [Header("Data")]
    private List<CardElement> _cards = new List<CardElement>();
    public List<CardElement> cards => _cards;

    private StackData _data;

    private int _layer;

    [Header("References")]
    [SerializeField] private StackCounter m_StackCounter;

    public void SetUp(StackData stackData, int layer)
    {
        _data = stackData;
        _layer = layer;

        for (int i = 0; i < _data.cardDatas.Count; i++)
        {
            SpawnCardElement(_data.cardDatas[i], i);
        }

        m_StackCounter.SetText(cards.Count);
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

                cardElement.SetUp(value, _layer, index, postion);
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
}
