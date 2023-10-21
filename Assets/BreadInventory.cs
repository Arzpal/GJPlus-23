using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreadInventory : MonoBehaviour
{
    [SerializeField] private RectTransform breadInv;
    [SerializeField] private float moveAmount = 400f;
    [SerializeField] private TMP_Text text;
    private bool open = false;

    public void movePanel()
    {
        if (!open) { MoverPanelIzquierda(); }
        else { MoverPanelDerecha(); }
    }
    public void MoverPanelIzquierda()
    {
        Vector2 currentPosition = breadInv.anchoredPosition;
        float newX = currentPosition.x - moveAmount;
        breadInv.anchoredPosition = new Vector2(newX, currentPosition.y);
        text.text = ">";
        open = true;
    }
    
    public void MoverPanelDerecha()
    {
        Vector2 currentPosition = breadInv.anchoredPosition;
        float newX = currentPosition.x + moveAmount;
        breadInv.anchoredPosition = new Vector2(newX, currentPosition.y);
        text.text = "<";
        open = false;
    }
}
