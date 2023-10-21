using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rt;
    private bool falling = false;
    [SerializeField] private float fallingSpeed = 0.2f;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        falling = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        falling = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Debug.Log("Drag");
    }

    private void Update()
    {
        if (falling)
        {
            var anchoredPosition = rt.anchoredPosition;
            anchoredPosition = new Vector2(anchoredPosition.x,anchoredPosition.y - (1*fallingSpeed));
            rt.anchoredPosition = anchoredPosition;
        }
    }
}
