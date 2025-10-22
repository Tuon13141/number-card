using TMPro;
using UnityEngine;

public class CardElement : MonoBehaviour
{
    [Header("Card Values")]
    int _cardValue = 0;

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
}
