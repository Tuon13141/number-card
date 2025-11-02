using UnityEngine.EventSystems;

public interface IDraggable : IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler { }
public interface ITapable : IPointerClickHandler { }