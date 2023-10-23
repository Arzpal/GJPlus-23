using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private List<int> ventas;
    public List<GameObject> objetos;
    public int price;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("estoy entrando");
        ventas.Add(col.GetComponent<DragDrop>().breadtype);
        objetos.Add(col.gameObject);
        price += col.GetComponent<DragDrop>().price;
        col.GetComponent<DragDrop>().falling = 2;
    }

    private void OnTriggerStay2D(Collider2D col)
	{
        col.GetComponent<DragDrop>().falling = 2;
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        objetos.Remove(other.gameObject);
        other.GetComponent<DragDrop>().falling = 0;
        ventas.Remove(other.GetComponent<DragDrop>().breadtype);
        price -= other.GetComponent<DragDrop>().price;
    }

    
    public List<int> getVentas()
    {
        return ventas;
    }

    public int getPrice()
    {
        int price = 0;
        
        for (int i = 0; i < ventas.Count; i++)
        {
            switch (ventas[i])
            {
                case 0:
                    price += 5;
                    break;
                case 1:
                    price += 3;
                    break;
                case 2:
                    price += 1;
                    break;
            }
        }
        return price;
    }
    
    public int getPriceWithTaxes()
    {
        int price = 0;
        for (int i = 0; i < ventas.Count; i++)
        {
            switch (ventas[i])
            {
                case 0:
                    price += 3;
                    break;
                case 1:
                    price += 2;
                    break;
                case 2:
                    price += 1;
                    break;
            }
        }
        return getPrice() + price;
    }
}
