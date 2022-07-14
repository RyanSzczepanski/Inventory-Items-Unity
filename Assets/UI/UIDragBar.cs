using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragBar : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private Vector2 offset;
    public void OnDrag(PointerEventData eventData)
    {
        //transform.parent.GetComponent<RectTransform>().anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
        transform.parent.GetComponent<RectTransform>().position = eventData.position - offset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = eventData.position - (Vector2)transform.parent.GetComponent<RectTransform>().position;
        transform.parent.SetAsLastSibling();
        
    }
}
