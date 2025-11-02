using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour, IDraggable
{
    public Action OnBeginDragAction;
    public Action OnDragAction;
    public Action OnEndDragAction;
    public Action OnPointerDownAction;
    public Action OnPointerUpAction;

    [SerializeField] public DragableSetting dragableSetting;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!dragableSetting.canInteract) return;

        OnBeginDragAction?.Invoke();
    }

    Tween _scaleTween;

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragableSetting.canInteract) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = transform.position.z;
        transform.position = worldPos + (Vector3)dragableSetting.offset;

        OnDragAction?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragableSetting.canInteract) return;

        OnEndDragAction?.Invoke();
    }

    public void EnableInteract() { dragableSetting.canInteract = true; }
    public void DisableInteract() { dragableSetting.canInteract = false; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!dragableSetting.canInteract) return;

        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(dragableSetting.scaleOnHold, dragableSetting.timeScale);

        OnPointerDownAction?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!dragableSetting.canInteract) return;

        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(Vector3.one, dragableSetting.timeScale);

        OnPointerUpAction?.Invoke();
    }

    public void SetInteractable(bool interactable)
    {
        dragableSetting.canInteract = interactable;
    }
}

[Serializable]
public class DragableSetting
{
    public bool canInteract = true;
    public Vector2 offset = Vector2.zero;

    public float timeScale = 0;
    public Vector2 scaleOnHold = Vector2.one;
}