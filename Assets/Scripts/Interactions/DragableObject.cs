using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour, IDraggable
{
    public Action OnBeginDragAction;
    public Action OnDragAction;
    public Action OnEndDragAction;

    [SerializeField] public DragableSetting dragableSetting;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!dragableSetting.canInteract) return;

        OnBeginDragAction?.Invoke();
    }

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
}

[Serializable]
public class DragableSetting
{
    public bool canInteract = true;
    public Vector2 offset = Vector2.zero;
}