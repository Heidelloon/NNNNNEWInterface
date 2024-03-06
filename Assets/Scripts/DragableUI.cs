using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class DragableUI : MonoBehaviour, IDragHandler,IBeginDragHandler
{
    public Transform DragUI; //²ø°í´Ù´Ò UI

    private Vector2 dragOffset;

    void Start()
    {
        if (DragUI == null)
            DragUI = transform;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2) DragUI.position - eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        DragUI.position = eventData.position + dragOffset;
    }
 
}
