using UnityEngine.EventSystems;

public interface IDraggable : IDragHandler, IBeginDragHandler, IEndDragHandler { }
public interface ITapable : IPointerClickHandler { }