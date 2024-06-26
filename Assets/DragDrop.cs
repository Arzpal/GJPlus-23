using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rt;
    public int falling = 0;
    [SerializeField] private float fallingSpeed = 0.2f;
    [SerializeField] private float fallingSpeedMax = 0.2f;
    [SerializeField] private string canvasName;
    [SerializeField] public int breadtype;
    private CanvasGroup cg;
    [SerializeField] public int price;
    public BreadInventory breadInv;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        Canvas canvasEncontrado = GameObject.Find(canvasName)?.GetComponent<Canvas>();
        cg = GetComponent<CanvasGroup>();

        if (canvasEncontrado != null)
        {
            Debug.Log("Canvas encontrado: " + canvasEncontrado.name);
            canvas = canvasEncontrado;
        }
        else
        {
            Debug.LogWarning("Canvas no encontrado: " + canvasName);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        int number = 0;
        if (breadtype == 0)
        {
            number = 7;
            breadInv.crustSoundID = number;
        }
        else number = breadInv.selectRandomNumber(4, 6, true);


        SoundController.Instance.PlaySound(1, number, gameObject.GetComponent<AudioSource>());

        falling = 0;
        cg.alpha = .6f;
        cg.blocksRaycasts = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        canvas.GetComponentInChildren<BreadInventory>().GrabbedBread(breadtype, gameObject.GetComponent<Image>());
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (falling == 0)
        {
            falling = 1;
        }
		else
        {
            int id = breadInv.selectRandomNumber(0, 3, false);
            SoundController.Instance.PlaySound(1, id, gameObject.GetComponent<AudioSource>());
        }
        cg.alpha = 1f;
        cg.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    private void Update()
    {
        if (falling == 1)
        {
            var anchoredPosition = rt.anchoredPosition;
            anchoredPosition = new Vector2(anchoredPosition.x,anchoredPosition.y - (1*fallingSpeed));
            rt.anchoredPosition = anchoredPosition; 
        }
    }
}
