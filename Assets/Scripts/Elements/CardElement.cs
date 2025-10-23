using System;
using TMPro;
using UnityEngine;

public class CardElement : MonoBehaviour
{
    [Header("Card Values")]
    int _cardValue = 0;
    public int cardValue => _cardValue;

    [Header("Card Settings")]
    int _layer = 0;
    Vector2 _position = Vector2.zero;

    [Header("References")]
    [SerializeField] DragableObject _dragableObject;
    [SerializeField] SpriteRenderer _cardSprite;
    [SerializeField] TextMeshPro _valueText;
    [SerializeField] BoxCollider2D _collider2D;

    public virtual void SetUp(int cardValue, int layer, Vector2 position)
    {
        _cardValue = cardValue;
        _layer = layer;
        _position = position;

        SetUpSprite();
    }

    protected virtual void SetUpSprite()
    {

    }

    protected void SetUpText()
    {
        _valueText.text = _cardValue.ToString();
    }


    protected virtual void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CardElement cardElement = collision.GetComponent<CardElement>();
        if (cardElement != null)
        {
            OnCardCollision(cardElement);
        }
    }

    protected virtual void OnCardCollision(CardElement otherCard)
    {

    }

    protected virtual bool CheckCardPair(CardElement otherCard)
    {
        if(otherCard == null) return false;

        int otherCardValue = otherCard.cardValue;

        if(otherCardValue == _cardValue) return true;
        if(otherCardValue + _cardValue == 10) return true;

        return false;
    }
}
