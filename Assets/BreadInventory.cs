using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreadInventory : MonoBehaviour
{
    [SerializeField] private Canvas canva;
    [SerializeField] private RectTransform breadInv;
    [SerializeField] private float moveAmount = 400f;
    [SerializeField] private TMP_Text text;
    private bool open = false;

    [SerializeField] private List<RectTransform> positions;
    [SerializeField] private List<Image> prefabs;
    [SerializeField] private List<int> quantitys;
    public int thisbread;

    private void Start()
    {
        throw new NotImplementedException();
    }

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

    public void GrabbedBread(int type)
    {
        Image nuevoObjetoHijo = Instantiate(prefabs[type], canva.transform, true);
        nuevoObjetoHijo.rectTransform.position = positions[type].position;
        quantitys[type] --;
        positions[type].GetComponentInChildren<TMP_Text>().text = "x " + quantitys[type];
    }
}
