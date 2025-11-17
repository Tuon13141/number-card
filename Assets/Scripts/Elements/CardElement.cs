using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CardElement : MonoBehaviour
{
    [Header("Card Data")]
    int _cardValue = 0;
    public int cardValue => _cardValue;


    int _cardIndex = 0;
    public int cardIndex => _cardIndex;


    private Vector2Int _gridPosition;
    public Vector2Int gridPosition => _gridPosition;


    private List<CardElement> _upperCards = new List<CardElement>();
    private List<CardElement> _lowerCards = new List<CardElement>();


    [HorizontalLine(2, EColor.Blue)]
    [Header("Card Settings")]
    int _layer = 0;
    Vector2 _startPosition = Vector2.zero;
    public const float timeReturnToStartPosition = 0.5f;

    bool _canInteractable = true;
    bool _isPicking = false;


    [HorizontalLine(2, EColor.Blue)]
    [Header("References")]
    [SerializeField] DragableObject m_DragableObject;
    [SerializeField] SpriteRenderer m_CardSprite;
    [SerializeField] TextMeshPro m_ValueText;
    [SerializeField] BoxCollider2D m_Collider2D;

    private LayerElement _layerElement;
    private CardElement _collisionCard;

    protected virtual void Start()
    {
        m_DragableObject.OnPointerDownAction += OnStartDrag;
        m_DragableObject.OnDragAction += OnDrag;
        m_DragableObject.OnPointerUpAction += OnEndDrag;
    }


    public virtual void SetUp(CardData cardData, Vector2 position, LayerElement layerElement)
    {
        _cardValue = cardValue;
        _layer = layerElement.layerIndex;
        _cardIndex = cardIndex;
        _layerElement = layerElement;

        transform.parent = layerElement.transform;
        transform.localPosition = position;

        _startPosition = position;

        SetUpSprite();
        SetUpText();
    }

    protected virtual void SetUpSprite()
    {
        m_CardSprite.sortingOrder = _layer;
        m_ValueText.sortingOrder = _layer;
    }

    protected void SetUpText()
    {
        m_ValueText.text = _cardValue.ToString();
    }

    #region Merge

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(_collisionCard != null) return;
        if (!_isPicking) return; 

        CardElement cardElement = collision.GetComponent<CardElement>();
        if (cardElement != null)
        {
            if (CheckCardPair(cardElement))
            {
                _collisionCard = cardElement;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isPicking) return;

        CardElement cardElement = collision.GetComponent<CardElement>();
        if(cardElement == null || cardElement != _collisionCard)
        {
            return;
        }

        OnResetSprite();
    }

    protected virtual void MergeCard()
    {
        _collisionCard.OnMerge();
        OnMerge();
    }

    public void OnMerge()
    {
        _layerElement.OnCardMerge(this);
    }

    protected virtual bool CheckCardPair(CardElement otherCard)
    {
        if(otherCard == null) return false;

        if(!otherCard.CheckConditionToMerge()) return false;

        int otherCardValue = otherCard.cardValue;

        if(otherCardValue == _cardValue) return true;
        if(otherCardValue + _cardValue == 10) return true;

        return false;
    }

    #endregion

    #region CheckCondition

    public bool CheckConditionToMerge()
    {
        return CheckUpperStack();
    }

    public bool CheckUpperStack()
    {
        bool b = true;

        if (_upperCards.Count > 0)
        {
            b = false;
        }

        return b;
    }

    public void CheckLowerStack()
    {
        foreach (CardElement cardElement in _lowerCards)
        {
            if (cardElement.CheckUpperStack())
            {
                cardElement.Unlock();
            }
        }
    }

    public bool CheckCanInteractable()
    {
        if (!_canInteractable) return false;
        return true;
    }
    #endregion

    #region Interaction

    Tween _moveTween;
    void ReturnToStartPosition()
    {
        _moveTween?.Kill();
        _moveTween = transform.DOMove(_startPosition, timeReturnToStartPosition);
    }

    void OnStartDrag()
    {
        if (!CheckCanInteractable()) return;

        _isPicking = true;
        SetInteractionSprite();
        m_CardSprite.sortingLayerName = "Picking";
    }

    void OnEndDrag()
    {
        if (!CheckCanInteractable()) return;

        SetSpriteColor(Color.white);

        if (_collisionCard != null)
        {
            MergeCard();
        }
        else
        {
            ReturnToStartPosition();
        }

        m_CardSprite.sortingLayerName = "Default";

        int layerId = SortingLayer.NameToID("Default");
        m_ValueText.sortingLayerID = layerId;

        _isPicking = false;
    }

    void OnDrag()
    {
        if (!CheckCanInteractable()) return;

        SetInteractionSprite();
    }

    void SetInteractionSprite()
    {
        if (_collisionCard != null)
        {
            SetSpriteColor(Color.green, 0.75f);
            _collisionCard.SetSpriteColor(Color.green);
        }
        else
        {
            SetSpriteColor(Color.white, 0.75f);
        }

        int layerId = SortingLayer.NameToID("Picking");
        m_ValueText.sortingLayerID = layerId;
    }


    #endregion

    #region Setter
    public void SetSpriteColor(Color color, float aphal = 1)
    {
        Color c = color;
        c.a = aphal;
        m_CardSprite.color = c;
    }

    public void Lock()
    {
        _canInteractable = false;
        SetSpriteColor(Color.cyan, 1f);
        m_DragableObject.SetInteractable(false);
    }

    public void Unlock()
    {
        _canInteractable = true;
        SetSpriteColor(Color.white, 1f);
        m_DragableObject.SetInteractable(true);
    }

    public void SetUpperCard(List<CardElement> upperElements)
    {
        _upperCards.AddRange(upperElements);

        if (upperElements.Count <= 0) return;
        Lock();
    }

    public void AddLowerStack(CardElement lowerElement)
    {
        _lowerCards.Add(lowerElement);
    }
    #endregion

    public void OnResetSprite()
    {
        _collisionCard?.SetSpriteColor(Color.white, 1);
        SetSpriteColor(Color.white, 1);
        _collisionCard = null;
    }

    public virtual void OnReset()
    {
        _cardValue = 0;
        _cardIndex = 0;
        _gridPosition = Vector2Int.zero;
        _upperCards = new List<CardElement>();
        _lowerCards = new List<CardElement>();

        _startPosition = Vector2.zero;

        _canInteractable = true;
        _isPicking = false;
        _layer = 0;

        OnResetSprite();

        this.transform.parent = null;

        SetUpText();

        PoolingManager.Instance.Release(this);
    }
}
